using ReactiveUI;

namespace Kaigara.Configuration.UI.ViewModels;

public record class OptionsPageMetadata
{
    public Type? ModelType { get; init; }
    public Type? CategoryType { get; init; }
    public string? Title { get; init; }
    public int DisplayOrder { get; init; }

    public void Deconstruct(out Type? modelType, out Type? categoryType, out string? title, out int displayOrder)
    {
        modelType = ModelType;
        categoryType = CategoryType;
        title = Title;
        displayOrder = DisplayOrder;
    }
}

public record class OptionCategory(string Label, int DisplayOrder = 0, Type? ParentType = null);
public record class OptionCategory<TParent>(string Label, int DisplayOrder = 0)
    : OptionCategory(Label, DisplayOrder, typeof(TParent))
    where TParent : OptionCategory
{

}

public abstract class OptionsPageViewModel : ReactiveObject
{
    protected OptionsPageViewModel()
    {

    }
}
