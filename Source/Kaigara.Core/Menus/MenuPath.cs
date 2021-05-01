using System.Collections.Generic;

namespace Kaigara.Menus
{
    public class MenuPath
    {
        private static readonly char[] pathSeparators = new char[] { '\\', '/' };
        private readonly string originalPath;
        private readonly IReadOnlyList<string> pathSegments;

        public MenuPath(string path)
        {
            originalPath = path ?? throw new System.ArgumentNullException(nameof(path));
            pathSegments = path.Split(pathSeparators);
        }

        public IReadOnlyList<string> PathSegments => pathSegments;
    }
}