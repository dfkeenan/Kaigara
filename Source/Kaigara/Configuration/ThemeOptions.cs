using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaigara.Configuration;
public class ThemeOptions : IOptionsModel
{
    public string? ThemeName { get; set; }
}
