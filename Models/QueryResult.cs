namespace SolrIrApp.Models;

public class QueryResult<T> {
    public IEnumerable<T> Results { get; private set; }
    public int TotalResults { get; private set; }

    public QueryResult(IEnumerable<T> results, int totalResults){
        Results = results;
        TotalResults = totalResults;
    }
}
