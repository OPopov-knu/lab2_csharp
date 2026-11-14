using CommunityToolkit.Maui.Storage;

namespace XmlLabApp.Services;

public static class XmlFileService
{
    private static readonly FilePickerFileType XmlType = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        [DevicePlatform.MacCatalyst] = ["public.xml"],
        [DevicePlatform.iOS] = ["public.xml"],
        [DevicePlatform.WinUI] = [".xml"],
        [DevicePlatform.Android] = ["text/xml", "application/xml"],
    });

    private static readonly FilePickerFileType XslType = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        [DevicePlatform.MacCatalyst] = ["public.data"],
        [DevicePlatform.iOS] = ["public.data"],
        [DevicePlatform.WinUI] = [".xsl"],
        [DevicePlatform.Android] = ["*/*"],
    });

    public static Task<FileResult?> PickXmlAsync() =>
        FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Оберіть XML-файл", FileTypes = XmlType });

    public static Task<FileResult?> PickXslAsync() =>
        FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Оберіть XSL-файл", FileTypes = XslType });

    public static async Task<string?> SaveHtmlAsync(Stream html, string suggestedName)
    {
        var result = await FileSaver.Default.SaveAsync(suggestedName, html, CancellationToken.None);
        if (result.IsSuccessful) return result.FilePath;
        if (result.Exception is null or OperationCanceledException) return null;
        if (result.Exception.Message.Contains("cancel", StringComparison.OrdinalIgnoreCase)) return null;
        throw new IOException($"Не вдалося зберегти файл: {result.Exception.Message}", result.Exception);
    }
}
