using Microsoft.AspNetCore.Components;

namespace ClinicApp.WebApp.Components;

public partial class ReportContent : ComponentBase
{
    [Parameter]
    public string Title { get; set; } = null!;
    
    [Parameter]
    public bool HiddenSendButton { get; set; } = true;

    [Parameter]
    public bool HiddenDownloadButton { get; set; } = true;
    
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;
    
    [Parameter]
    public Func<Task>? SendButtonAction { get; set; }
    
    [Parameter]
    public Func<Task>? DownloadButtonAction { get; set; }
}
