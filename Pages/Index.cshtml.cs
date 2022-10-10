using Microsoft.AspNetCore.Mvc.RazorPages;
using SolrIrApp.Models;
using SolrIrApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SolrIrApp.Pages;

public class IndexModel : PageModel
{
    private readonly Indexer<Book> _indexer;
    private readonly Searcher<Book> _searcher;
    private readonly IConfiguration _config;

    private readonly int PageSize;
    private readonly int PageWindow;

    public SelectList SelectOptions { get; private set; }

    [BindProperty]
    public IFormFileCollection? Files { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Field { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? QueryString { get; set; }

    [TempData]
    public int NumIndexed { get; set; }

    public IEnumerable<(Book, int)>? SearchResults { get; private set; }
    public int TotalResults { get; private set; }
    public Pagination? Pagination { get; private set; }

    public IndexModel(Indexer<Book> indexer, Searcher<Book> searcher, IConfiguration config)
    {
        _indexer = indexer;
        _searcher = searcher;
        _config = config;

        PageSize = Int32.Parse(_config["PageSize"]);
        PageWindow = Int32.Parse(_config["PageWindow"]);
        SelectOptions = new SelectList(Book.getSolrFieldsNames());
    }

    public void OnGet()
    {
    }

    public IActionResult OnGetSearch(string field, string queryString, int pageIndex = 1)
    {
        try{

            if(field == "dateIndexed") queryString = DateTime.Parse(queryString).Date.ToString("yyyy-MM-ddTHH:mm:ssZ");

            QueryResult<Book> res = _searcher.searchFieldValue(pageIndex - 1, PageSize, field, queryString);

            SearchResults = res.Results.Select((b, i) => (b, ((pageIndex - 1)  * PageSize) + i));
            TotalResults = res.TotalResults;
            RouteInfo routeInfo = new RouteInfo{
                Path = "",
                Action = "search",
                Query = new Dictionary<string, string>(){
                         {"field", field},
                         {"queryString", queryString},
                }
            };
            Pagination = new Pagination(pageIndex, (res.TotalResults + PageSize - 1) / PageSize, PageWindow, routeInfo);
        }
        catch(SolrNet.Exceptions.SolrConnectionException e){
            Console.WriteLine($"error onGetSearch: {e.Message}");
            return RedirectToPage("Error");
        }

        return Page();
    }

    public IActionResult OnPost(){

        if(Files != null)
            TempData["NumIndexed"] = _indexer.indexRange(Files.Select(file => Book.fromFile(file)));

        return RedirectToPage("Index");
    }
}
