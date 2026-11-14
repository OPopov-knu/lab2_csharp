using XmlLabApp.Models;

namespace XmlLabApp.ViewModels;

public class AttributeFilter(string name, IReadOnlyList<string> values) : BindableObject
{
    private string? _selectedValue;

    public string Name { get; } = name;
    public string Caption { get; } = Member.CaptionOf(name);
    public IReadOnlyList<string> Values { get; } = values;

    public string? SelectedValue
    {
        get => _selectedValue;
        set
        {
            _selectedValue = value;
            OnPropertyChanged();
        }
    }
}
