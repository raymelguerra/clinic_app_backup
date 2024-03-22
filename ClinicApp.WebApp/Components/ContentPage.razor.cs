using Microsoft.AspNetCore.Components;

namespace ClinicApp.WebApp.Components
{
    public partial class ContentPage : ComponentBase
    {
        [Parameter]
        public string Icon { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public bool HiddenAddButton { get; set; } = true;
        [Parameter]
        public bool HiddenDownloadButton { get; set; } = true;
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public Func<Task> AddButtonAction { get; set; }
        [Parameter]
        public Func<Task> DownloadButtonAction { get; set; }

        public string Concat(int value){
            var option = value == 0 ? "Add" : "Download";
            return $"{option} {Title}";
}
    }
}
