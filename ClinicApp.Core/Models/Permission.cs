
namespace ClinicApp.Core.Models
{
    public class Permission
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ParentMenu>? Parents { get; set; }
    }
}
