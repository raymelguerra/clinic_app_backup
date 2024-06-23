using ClinicApp.Api.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.Extensions.Options;

namespace ClinicApp.Api.Services
{
    public class MenusService(IOptions<MenuConfiguration> _options) : IMenusService
    {
        private readonly MenuConfiguration menu = _options.Value;

        public List<ParentMenu>? GetMenusByRole(string[] roles)
        {
            //var result = new List<ParentMenu>();

            //foreach (var item in menu.Menus)
            //{
            //    var parentAux = new ParentMenu
            //    {
            //        Name = item.Name,
            //        Index = item.Index,
            //        Disabled = item.Disabled,
            //        Icon = item.Icon,
            //        Childrens = new List<ChildMenu>()
            //    };

            //    foreach (var child in item.Childrens)
            //    {
            //        roles.ForEach(x =>
            //        {
            //            if (child.Roles.Contains(x))
            //            {
            //                parentAux.Childrens.Add(new ChildMenu
            //                {
            //                    Name = child.Name,
            //                    Index = child.Index,
            //                    Path = child.Path,
            //                    Disabled = child.Disabled,
            //                    Icon = child.Icon,
            //                    Roles = child.Roles,
            //                    Childrens = child.Childrens
            //                });
            //            }
            //        });
            //    }
            //    if (parentAux.Childrens.Count > 0)
            //        result.Add(parentAux);
            //}

            //return result;

            return menu.Menus
        .Select(item => new ParentMenu
        {
            Name = item.Name,
            Index = item.Index,
            Disabled = item.Disabled,
            Icon = item.Icon,
            Childrens = item.Childrens
                .Where(child => child.Roles.Any(role => roles.Contains(role)))
                .Select(child => new ChildMenu
                {
                    Name = child.Name,
                    Index = child.Index,
                    Path = child.Path,
                    Disabled = child.Disabled,
                    Icon = child.Icon,
                    Roles = child.Roles,
                    Childrens = child.Childrens
                })
                .ToList()
        })
        .Where(parent => parent.Childrens.Any())
        .ToList();
        }
    }
}
