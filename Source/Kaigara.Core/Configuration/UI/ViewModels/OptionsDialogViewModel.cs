using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Avalonia.Controls;
using Kaigara.Collections.Generic;
using Kaigara.Dialogs.ViewModels;
using ReactiveUI;

using LazyPage = System.Lazy<Kaigara.Configuration.UI.ViewModels.OptionsPageViewModel, Kaigara.Configuration.UI.ViewModels.OptionsPageMetadata>;

namespace Kaigara.Configuration.UI.ViewModels;
public class OptionsDialogViewModel : DialogViewModel<IEnumerable<OptionsDialogResultItem>>
{

    public OptionsDialogViewModel(ConfigurationManager configurationManager,IEnumerable<LazyPage> pages)
    {
        Title = "Options";
        MinWidth = 800; 
        MinHeight = 500;

        this.configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));

        optionsPages = new SortedSet<OptionsPageItem>(OptionsPageItemDisplayComparer.Instance);
        var categoryItems = new Dictionary<Type, OptionsPageItem>();

        OptionsPageItem? GetCategoryItem(Type categoryType)
        {
            if (categoryItems.TryGetValue(categoryType, out var categoryItem))
            {
                return categoryItem;
            }

            OptionsPageItem? result = null;
            OptionsPageItem? root = null;

            while (categoryType != null && categoryType.IsAssignableTo(typeof(OptionCategory)))
            {
                if (categoryType?.GetConstructor(Type.EmptyTypes) is null) break;

                if (!categoryItems.TryGetValue(categoryType, out categoryItem))
                {
                    var category = (OptionCategory)System.Activator.CreateInstance(categoryType)!;
                    categoryItem = new OptionsPageItem(category.Label, category.DisplayOrder);
                    categoryItems.Add(categoryType, categoryItem);

                    categoryType = category.ParentType!;
                }

                if (root is not null)
                {
                    categoryItem.Children.Add(root);
                }

                root = categoryItem;
                result ??= categoryItem;
            }

            if (root is not null)
            {
                optionsPages.Add(root);
            }

            return result;
        }

        foreach (var page in pages.Where(p => p.Metadata is { Title : not null, CategoryType: not null, ModelType: not null}))
        {
            var (modelType, categoryType, title, displayOrder) = page.Metadata;

            var item = new OptionsPageItem( page.Metadata.Title!, page.Metadata.DisplayOrder, page);

            GetCategoryItem(categoryType!)?.Children.Add(item);
        }

        FilteredPages = optionsPages;
    }

    private readonly Dictionary<OptionsPageViewModel, OptionsPageMetadata> metadata = new();
    private readonly HashSet<OptionsPageViewModel> materializedPageViewModel = new();
    private readonly HashSet<OptionsPageViewModel> changedPageViewModels = new();
    private readonly HashSet<OptionsPageViewModel> boundPageViewModels = new();
    private readonly SortedSet<OptionsPageItem> optionsPages;
    private readonly ConfigurationManager configurationManager;

    public IEnumerable<OptionsPageItem> FilteredPages { get;  set; }


    private OptionsPageItem? currentPageItem;
    public OptionsPageItem? CurrentPageItem
    {
        get => currentPageItem;
        set
        {
            this.RaiseAndSetIfChanged(ref currentPageItem, value);

            if(value == null) return;

            var page = value.Page ?? GetChildren(value.Children).FirstOrDefault(p => p.Page is not null)?.Page;

            if (page is not null)
            {
                CurrentPage = MaterializePage(page);
            }
        }
    }

    private OptionsPageViewModel? currentPage;
    public OptionsPageViewModel? CurrentPage 
    { 
        get => currentPage; 
        private set => this.RaiseAndSetIfChanged(ref currentPage, value);
    }

    public Task Ok()
    {
        var result = changedPageViewModels
                        .Select(p => new OptionsDialogResultItem(p, metadata[p]))
                        .ToList();
        return Close(result);
    }

    public Task Cancel()
    {
        return Close(Enumerable.Empty<OptionsDialogResultItem>());
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        base.OnActivated(disposables);

        if (CurrentPageItem?.Page is LazyPage page)
        {
            CurrentPage = MaterializePage(page);
        }
        else
        {
            CurrentPageItem ??= GetChildren(optionsPages).FirstOrDefault(p => p.Page is not null);
        }
    }
    protected override void OnDispose()
    {
        base.OnDispose();

        changedPageViewModels.Clear();
        boundPageViewModels.Clear();
    }

    private OptionsPageViewModel MaterializePage(LazyPage lazyPage)
    {
        var viewModel = lazyPage.Value;

        if (materializedPageViewModel.Add(viewModel))
        {
            metadata.Add(viewModel, lazyPage.Metadata);
            viewModel.Changed
                .Select(e => (OptionsPageViewModel)e.Sender)
                .Subscribe(vm =>
                {
                    changedPageViewModels.Add(vm);
                });
        }

        if (boundPageViewModels.Add(viewModel))
        {
            configurationManager.BindSection(lazyPage.Metadata.ModelType!, viewModel);
        }

        return viewModel;
    }

    private static IEnumerable<OptionsPageItem> GetChildren(IEnumerable<OptionsPageItem> pages)
    {
        foreach (var page in pages)
        {
            yield return page;

            foreach (var item in GetChildren(page.Children))
            {
                yield return item;
            }
        }
    }
}

public record class OptionsPageItem(string Label, int DisplayOrder, LazyPage? Page = null)
{
    public ICollection<OptionsPageItem> Children { get; }
        = new SortedSet<OptionsPageItem>(OptionsPageItemDisplayComparer.Instance);

    public bool IsExpanded { get; set; }
}

internal class OptionsPageItemDisplayComparer : IComparer<OptionsPageItem>
{

    public static OptionsPageItemDisplayComparer Instance { get; } = new (); 

    public int Compare(OptionsPageItem? x, OptionsPageItem? y)
    {
        var result = Comparer<int?>.Default.Compare(x?.DisplayOrder, y?.DisplayOrder);

        if (result != 0) return result;

        return StringComparer.CurrentCulture.Compare(x?.Label, y?.Label);
    }
}

public record struct OptionsDialogResultItem(OptionsPageViewModel ViewModel, OptionsPageMetadata Metadata);
