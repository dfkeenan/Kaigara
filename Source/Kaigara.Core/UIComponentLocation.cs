using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaigara
{
    public class UIComponentLocation
    {
        private static readonly char[] pathSeparators = new char[] { '\\', '/' };
        private readonly string originalPath;
        private readonly IReadOnlyList<string> pathSegments;

        public UIComponentLocation(string path)
        {
            originalPath = path ?? throw new ArgumentNullException(nameof(path));
            pathSegments = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public UIComponentLocation(IEnumerable<string> path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!path.Any())
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
