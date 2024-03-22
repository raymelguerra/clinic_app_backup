namespace ClinicApp.WebApp.Services;

public class NavigationService
{
    private List<string> _navigationHistory = new List<string>();

    public IReadOnlyList<string> NavigationHistory => _navigationHistory.AsReadOnly();

    public event Action OnNavigationHistoryChanged;

    public void NavigateTo(string url)
    {
        _navigationHistory.Add(url);
        OnNavigationHistoryChanged?.Invoke();
    }

    public void ClearHistory()
    {
        _navigationHistory.Clear();
        OnNavigationHistoryChanged?.Invoke();
    }
}
