namespace Kaigara;

public class UIComponentLocation
{
    private static readonly char[] pathSeparators = new char[] { '\\', '/' };
    private readonly string originalPath;
    private readonly IReadOnlyList<string> pathSegments;

    public UIComponentLocation(string path)
    {
        if(string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path));


        originalPath = path;

        pathSegments = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IsRelative = pathSeparators.Contains(path[0]);
    }
    
    public IReadOnlyList<string> PathSegments => pathSegments;

    public bool IsRelative { get; }

    public override string ToString() => originalPath;
}
