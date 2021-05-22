using System.Collections.Generic;
using System;
using System.Linq;

namespace Kaigara.Menus
{
    public class MenuPath
    {
        private static readonly char[] pathSeparators = new char[] { '\\', '/' };
        private readonly string originalPath;
        private readonly IReadOnlyList<string> pathSegments;

        public MenuPath(string path)
        {
            originalPath = path ?? throw new ArgumentNullException(nameof(path));
            pathSegments = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public MenuPath(IEnumerable<string> path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(!path.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(path), "Must have at least 1 item");
            }

            originalPath = String.Join('/', path);
            pathSegments = path.ToArray();
        }

        public IReadOnlyList<string> PathSegments => pathSegments;

        public override string ToString() => originalPath;
    }
}