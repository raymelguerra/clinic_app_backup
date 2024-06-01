
namespace ClinicApp.Core.Models
{
    public class ParentMenu
    {
        public string? Name { get; set; }
        public int Index { get; set; }
        public bool Disabled { get; set; } = false;
        public string? Icon { get; set; }
        public List<ChildMenu> Childrens { get; set; } = null!;
    }
}
