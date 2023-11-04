namespace Kaigara.Configuration;
public class ConfigurationModel
{
    private List<IOptionsModel> options;

    public ConfigurationModel(IEnumerable<IOptionsModel> options)
    {
        this.options = options.ToList();
    }

    public IEnumerable<IOptionsModel> Options => options;
}
