namespace Kaigara.Configuration.UI.ViewModels;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class OptionsPageAttribute(Type modelType, Type categoryType, string title) : Attribute
{
    public Type ModelType { get; }
        = modelType is Type t && t.IsAssignableTo(typeof(IOptionsModel)) ? modelType
        : throw new ArgumentException($"Must implmement {typeof(IOptionsModel)}", nameof(modelType));

    public Type CategoryType { get; }
        = categoryType is Type t && t.IsAssignableTo(typeof(OptionCategory)) ? categoryType
        : throw new ArgumentException($"Must inhert {typeof(OptionCategory)}", nameof(categoryType));
    public string Title { get; } = title ?? throw new ArgumentNullException(nameof(title));

    public int DisplayOrder { get; init; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class OptionsPageAttribute<TModel, TCategory>(string title) : OptionsPageAttribute(typeof(TModel), typeof(TCategory), title)
    where TModel : IOptionsModel
    where TCategory : OptionCategory
{ }