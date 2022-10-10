using SolrNet;

namespace SolrIrApp.Services;
public class Indexer<T>{

    private ISolrOperations<T> _solr;

    public Indexer(ISolrOperations<T> solr){
        _solr = solr;
    }

    public int indexRange(IEnumerable<T> data){
        try{
            _solr.AddRange(data);
            _solr.Commit();
        }
        catch(SolrNet.Exceptions.SolrConnectionException e){
            Console.WriteLine("indexRange error: " + e.Message);
        }

        return data.Count();
    }

    public int indexSingle(T data){
        try{
            _solr.Add(data);
            _solr.Commit();
        }
        catch(SolrNet.Exceptions.SolrConnectionException e){
            Console.WriteLine("indexSingle error: " + e.Message);
        }

        return 1;
    }
}
