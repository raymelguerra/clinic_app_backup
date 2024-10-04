using Microsoft.AspNetCore.Components;
using Oauth2.sdk;
using System.Net;

namespace ClinicApp.WebApp.Services
{
    public abstract class HttpClientServiceBase
    {
        protected readonly HttpClient client;
        protected readonly NavigationManager navigationManager;
        protected readonly IUserManagementService userIdpManagement;
        protected HttpClientServiceBase(
            IHttpClientFactory _factory,
            NavigationManager _navigationManager,
            IUserManagementService _userIdpManagement
        )
        {
            client = _factory.CreateClient("CustomClient");
            navigationManager = _navigationManager;
            userIdpManagement = _userIdpManagement;
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization =
                  new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userIdpManagement.GetToken());
                Console.WriteLine(userIdpManagement.GetToken());

                var response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    navigationManager.NavigateTo($"Login?redirectUri={Uri.EscapeDataString(navigationManager.Uri)}", true);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    navigationManager.NavigateTo("/forbidden");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception($"Api error response: HttpCode: {response.StatusCode}");

                return response;
            }
            catch (Exception ex)
            {
                //logging
                throw;
            }
        }

        protected async Task<Stream> SendAsyncGetStream(HttpRequestMessage request)
        {
            try
            {
                // client.DefaultRequestHeaders.Authorization =
                //            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userIdpManagement.GetToken());

                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    navigationManager.NavigateTo($"Login?redirectUri={Uri.EscapeDataString(navigationManager.Uri)}", true);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    navigationManager.NavigateTo("/forbidden");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception($"Api error response: HttpCode: {response.StatusCode}");

                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                //logging
                throw;
            }
        }
    }
}
