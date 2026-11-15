# Лабораторна робота № 2 — робота з файлами XML (варіант 28)

Тематика: «Студентський парламент». Аналіз XML-файлу (способами SAX API, DOM API, LINQ to XML) і трансформація у файл HTML на основі XSL-документа.

## Структура

| Папка | Вміст |
|---|---|
| `XmlLabApp/Strategies` | шаблон «Стратегія»: `IXmlSearchStrategy` і реалізації `SaxSearchStrategy`, `DomSearchStrategy`, `LinqSearchStrategy` |
| `XmlLabApp/Models` | дані запису й пошуковий запит |
| `XmlLabApp/Services` | XSL-трансформація, вибір і збереження файлів, діалоги |
| `XmlLabApp/ViewModels` | логіка інтерфейсу (MVVM, команди замість обробників кнопок) |
| `Samples` | файли-приклади `student_parliament.xml` і `student_parliament.xsl` |
| `docs` | UML-діаграми (PNG): прецедентів, класів, послідовності |

У .NET немає класичного SAX, тому його роль виконує потоковий `XmlReader`.

## Запуск

```bash
dotnet run --project XmlLabApp -f net10.0-windows10.0.19041.0
```
