using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Net;

/*
 *    This service is responsible for fetching the menus from the API
 *       and returning them to the components that need them.
 */
namespace ClinicApp.WebApp.Services
{
    public class AppMenusService(
       IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IAppMenusService
    {
        private readonly ApiSettings apiSettings = options.Value;


        public async Task<IEnumerable<ParentMenu>> GetMenusAsync()
        {
            var role = userIdpManagement.GetUserRoles();
            if (role == null || role.Count() == 0)
            {
                return Array.Empty<ParentMenu>();
            }
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Menu/{string.Join(",", role)}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ParentMenu>>(
                result) ?? Array.Empty<ParentMenu>();
        }
    }
}
