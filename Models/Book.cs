using SolrNet.Attributes;
using SolrIrApp.ExtensionMethods;

namespace SolrIrApp.Models;

public class Book{

    [SolrField("id")]
    public string? Id { get; set; }

    [SolrField("title")]
    public string? Title { get; set; }

    [SolrField("link")]
    public string? Link { get; set; }

    [SolrField("content")]
    public string? Content { get; set; }
    
    [SolrField("size")]
    public long Size { get; set; }

    [SolrField("dateIndexed")]
    public DateTime DateIndexed { get; set; }

    public static Book fromFile(IFormFile f){

        Book b = new Book{
            Id = Guid.NewGuid().ToString(),
            Title = f.FileName.stripExtension().Replace('_', ' '),
            Link = System.IO.Path.Join(Startup.StaticConfiguration["BooksPath"], f.FileName),
            Size = f.Length,
            DateIndexed = DateTime.Now.Date.AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now.Date).Hours)
        };
        
        try{
            using(StreamReader reader = new StreamReader(f.OpenReadStream())){
                b.Content = reader.ReadToEnd();
            }
        }
        catch(IOException e){
            Console.WriteLine($"error reading file: {e.Message}");
        }

        return b;
    }

    public static IEnumerable<string> getSolrFieldsNames(){
        return typeof(Book).GetProperties()
            .SelectMany(prop => prop.GetCustomAttributes(true))
            .Where(attr => attr is SolrFieldAttribute)
            .Select(attr => (attr as SolrFieldAttribute).FieldName);
    }

    public override string ToString(){
        return $"{Title} at {Link}";
    }
}
