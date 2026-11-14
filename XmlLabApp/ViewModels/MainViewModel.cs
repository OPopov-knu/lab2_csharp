using System.Collections.ObjectModel;
using System.Windows.Input;
using XmlLabApp.Models;
using XmlLabApp.Services;
using XmlLabApp.Strategies;

namespace XmlLabApp.ViewModels;

public class MainViewModel : BindableObject
{
    private readonly Command _searchCommand;
    private readonly Command _transformCommand;

    private string? _xmlPath;
    private string _fileName = "Файл не обрано";
    private IXmlSearchStrategy _selectedStrategy;
    private string _keywords = "";
    private string _output = "";

    public MainViewModel()
    {
        Strategies = [new SaxSearchStrategy(), new DomSearchStrategy(), new LinqSearchStrategy()];
        _selectedStrategy = Strategies[0];

        _searchCommand = new Command(() => _ = RunAsync(SearchAsync), () => _xmlPath != null);
        _transformCommand = new Command(() => _ = RunAsync(TransformAsync), () => _xmlPath != null);
        OpenXmlCommand = new Command(() => _ = RunAsync(OpenXmlAsync));
        ClearCommand = new Command(Clear);
        ExitCommand = new Command(() => _ = Dialogs.RequestExitAsync());
    }

    public IReadOnlyList<IXmlSearchStrategy> Strategies { get; }
    public ObservableCollection<AttributeFilter> Filters { get; } = [];

    public ICommand OpenXmlCommand { get; }
    public ICommand SearchCommand => _searchCommand;
    public ICommand TransformCommand => _transformCommand;
    public ICommand ClearCommand { get; }
    public ICommand ExitCommand { get; }

    public string FileName
    {
        get => _fileName;
        private set
        {
            _fileName = value;
            OnPropertyChanged();
        }
    }

    public IXmlSearchStrategy SelectedStrategy
    {
        get => _selectedStrategy;
        set
        {
            _selectedStrategy = value;
            OnPropertyChanged();
        }
    }

    public string Keywords
    {
        get => _keywords;
        set
        {
            _keywords = value;
            OnPropertyChanged();
        }
    }

    public string Output
    {
        get => _output;
        private set
        {
            _output = value;
            OnPropertyChanged();
        }
    }

    private async Task OpenXmlAsync()
    {
        var file = await XmlFileService.PickXmlAsync();
        if (file == null) return;

        var members = SelectedStrategy.Search(file.FullPath, SearchQuery.All);

        _xmlPath = file.FullPath;
        FileName = file.FileName;
        Clear();
        LoadFilters(members);
        _searchCommand.ChangeCanExecute();
        _transformCommand.ChangeCanExecute();
    }

    private void LoadFilters(IReadOnlyList<Member> members)
    {
        Filters.Clear();
        var attributes = members.SelectMany(m => m.Attributes).GroupBy(a => a.Key);
        foreach (var attribute in attributes)
        {
            var values = attribute.Select(a => a.Value).Distinct().Order(StringComparer.CurrentCulture).ToList();
            Filters.Add(new AttributeFilter(attribute.Key, values));
        }
    }

    private Task SearchAsync()
    {
        var found = SelectedStrategy.Search(_xmlPath!, BuildQuery());
        Output = found.Count == 0
            ? "Нічого не знайдено."
            : $"Спосіб: {SelectedStrategy.Name}. Знайдено записів: {found.Count}.{Environment.NewLine}{Environment.NewLine}" +
              string.Join(Environment.NewLine, found);
        return Task.CompletedTask;
    }

    private SearchQuery BuildQuery()
    {
        var attributeValues = Filters
            .Where(f => f.SelectedValue != null)
            .ToDictionary(f => f.Name, f => f.SelectedValue!);
        var keywords = Keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return new SearchQuery(attributeValues, keywords);
    }

    private async Task TransformAsync()
    {
        var xsl = await XmlFileService.PickXslAsync();
        if (xsl == null) return;

        using var html = XslHtmlTransformer.Transform(_xmlPath!, xsl.FullPath);
        string? savedTo = await XmlFileService.SaveHtmlAsync(html, Path.GetFileNameWithoutExtension(_xmlPath) + ".html");
        if (savedTo != null) Output = $"HTML-файл збережено: {savedTo}";
    }

    private void Clear()
    {
        foreach (var filter in Filters)
            filter.SelectedValue = null;
        Keywords = "";
        Output = "";
    }

    private static async Task RunAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            await Dialogs.ShowErrorAsync(ex.Message);
        }
    }
}
