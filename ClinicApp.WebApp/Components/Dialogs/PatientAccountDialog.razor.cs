using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PatientAccountDialog : ComponentBase
{
    [Parameter]
    public PatientAccount? Model { get; set; }

    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private PatientAccountValidator paValidator = new();
    private MudForm form = null!;
    private MudListItem<Agreement>? selectedItem;
    private object selectedValue
    {
        get { return Model; }
        set { Model = (PatientAccount)value; }
    }

    protected override Task OnParametersSetAsync()
    {
        if (Model == null)
            Model = new();
        return base.OnParametersSetAsync();
    }



    async void Edit()
    {
        await form.Validate();
        if (!form.IsValid) return;

        MudDialog.Close(DialogResult.Ok(Model));
    }
    void Cancel() => MudDialog.Cancel();
}
