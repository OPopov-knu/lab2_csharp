namespace XmlLabApp.Services;

public static class Dialogs
{
    private static bool _exitDialogOpen;

    private static Page CurrentPage => Application.Current!.Windows[0].Page!;

    public static Task ShowErrorAsync(string message) =>
        CurrentPage.DisplayAlertAsync("Помилка", message, "OK");

    public static async Task RequestExitAsync()
    {
        if (_exitDialogOpen) return;
        _exitDialogOpen = true;

        bool exit = await CurrentPage.DisplayAlertAsync(
            "Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");

        _exitDialogOpen = false;
        if (exit) Application.Current!.Quit();
    }
}
