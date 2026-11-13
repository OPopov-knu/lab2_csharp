using XmlLabApp.Models;

namespace XmlLabApp.Strategies;

public interface IXmlSearchStrategy
{
    string Name { get; }

    IReadOnlyList<Member> Search(string xmlPath, SearchQuery query);
}
