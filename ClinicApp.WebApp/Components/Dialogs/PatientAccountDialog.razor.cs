using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PatientAccountDialog : ComponentBase
{
    [Parameter]
    public IEnumerable<PatientAccount> PatientAccountList { get; set; }

    public PatientAccount Model { get; set; }

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private PatientAccountValidator paValidator = new();
    private MudForm form;
    private MudListItem selectedItem;
    private object selectedValue
    {
        get { return Model; }
        set { Model = (PatientAccount)value; }
    }

    protected override Task OnParametersSetAsync()
    {
        if (PatientAccountList != null && PatientAccountList.Count() > 0)
            Model = PatientAccountList.LastOrDefault();
        else Model = new();
        return base.OnParametersSetAsync();
    }



    void Edit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();
}
