using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.Models
{
    public class ChildMenu
    {
        public string? Name { get; set; }
        public int Index { get; set; }
        public string? Path { get; set; }
        public bool Disabled { get; set; } = false;
        public string? Icon { get; set; }
        public List<string> Roles { get; set; } = null!;
        public List<ChildMenu>? Childrens { get; set; }
    }
}
