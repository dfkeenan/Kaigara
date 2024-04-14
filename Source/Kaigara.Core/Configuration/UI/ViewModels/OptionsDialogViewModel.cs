﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Avalonia.Controls;
using Kaigara.Collections.Generic;
using Kaigara.Dialogs.ViewModels;
using ReactiveUI;

namespace Kaigara.Configuration.UI.ViewModels;
public class OptionsDialogViewModel : DialogViewModel<bool>
{

    public OptionsDialogViewModel(IEnumerable<Lazy<OptionsPageViewModel, OptionsPageMetadata>> pages)
    {
        Title = "Options";
        MinWidth = 800; 
        MinHeight = 500;

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
                }

                result ??= categoryItem;
                root = categoryItem;
                categoryType = categoryType.BaseType!;
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

        CurrentPage = GetChildren(optionsPages).FirstOrDefault(p => p.Page is not null)?.Page!.Value;

    }

    private readonly SortedSet<OptionsPageItem> optionsPages;

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
                CurrentPage = page.Value;
            }
        }
    }

    private OptionsPageViewModel? currentPage;
    public OptionsPageViewModel? CurrentPage 
    { 
        get => currentPage; 
        private set => this.RaiseAndSetIfChanged(ref currentPage, value);
    }

    public Task Ok() => Close(true);

    public Task Cancel() => Close(false);

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

public record class OptionsPageItem(string Label, int DisplayOrder, Lazy<OptionsPageViewModel>? Page = null)
{
    public ICollection<OptionsPageItem> Children { get; }
        = new SortedSet<OptionsPageItem>(OptionsPageItemDisplayComparer.Instance);
}

internal class OptionsPageItemDisplayComparer : IComparer<OptionsPageItem>
{

    public static OptionsPageItemDisplayComparer Instance { get; } = new (); 

    public int Compare(OptionsPageItem? x, OptionsPageItem? y)
    {
        var result = Comparer<int>.Default.Compare(x.DisplayOrder, y.DisplayOrder);

        if (result != 0) return result;

        return StringComparer.CurrentCulture.Compare(x.Label, y.Label);
    }
}
