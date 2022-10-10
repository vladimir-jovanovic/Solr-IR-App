namespace SolrIrApp.Models;

public class RouteInfo {

    public string? Path { get; set; }
    public string? Action { get; set; }

    public Dictionary<string, string> Query { get; set; }

    public string QueryValue {
        get{
            return string.Join('&', Query.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }

}
