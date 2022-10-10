using SolrNet;
using SolrNet.Commands.Parameters;

using SolrIrApp.Models;

namespace SolrIrApp.Services;

public class Searcher<T>
{

    private ISolrOperations<T> _solr;

    public Searcher(ISolrOperations<T> solr)
    {
        _solr = solr;
    }

    public QueryResult<T> searchFieldValue(int pageIndex, int pageSize, string field, string value)
    {
        QueryOptions queryOptions = new QueryOptions
        {
            Rows = pageSize,
            StartOrCursor = new StartOrCursor.Start(pageIndex * pageSize)
        };

        SolrQueryByField query = new SolrQueryByField(field, value);

        SolrQueryResults<T> res = _solr.Query(query, queryOptions);

        return new QueryResult<T>(res, (int)res.NumFound);
    }
}
