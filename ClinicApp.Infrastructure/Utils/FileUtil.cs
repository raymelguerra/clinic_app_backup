using Microsoft.AspNetCore.Components;

namespace ClinicApp.Infrastructure.Utils;
public static class FileUtil
{
    public static async Task SaveAs(NavigationManager navigationManager, string fileName, byte[] fileContent, string contentType)
    {
        if (contentType == "application/pdf")
        {
            var pdfDataUrl = $"data:{contentType};base64,{Convert.ToBase64String(fileContent)}";

            navigationManager.NavigateTo(pdfDataUrl, true);
        }
        else
        {
            var memoryStream = new MemoryStream(fileContent);
            var buffer = new byte[16 * 1024];
            var resultStream = new MemoryStream();
            int bytesRead;
            while ((bytesRead = await memoryStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await resultStream.WriteAsync(buffer, 0, bytesRead);
            }
            resultStream.Position = 0;

            navigationManager.NavigateTo($"data:{contentType};base64,{Convert.ToBase64String(resultStream.ToArray())}", true);
        }
    }
}
