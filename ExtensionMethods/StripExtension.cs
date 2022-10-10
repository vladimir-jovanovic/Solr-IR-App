
namespace SolrIrApp.ExtensionMethods;

public static class StripExtension
{
    public static string stripExtension(this string value){
        return value.Substring(0, value.LastIndexOf('.'));
    }
}
