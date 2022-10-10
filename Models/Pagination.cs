namespace SolrIrApp.Models;

public class Pagination {
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int PageWindow { get; private set; }

    public RouteInfo RouteInfo { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public IEnumerable<int> DisplayRange {
        get {

            if(TotalPages <= PageWindow) return Enumerable.Range(1, TotalPages);

            int start, end;

            if(PageIndex <= PageWindow / 2){
                start = 1;
                end = PageWindow;
            }
            else if((PageIndex + PageWindow / 2) >= TotalPages){
                start = TotalPages - PageWindow + 1;
                end = TotalPages;
            }
            else {
                start = PageIndex - PageWindow / 2;
                end = PageIndex + PageWindow / 2;
            }

            return Enumerable.Range(start, (end - start) + 1);
        }
    }

    public Pagination(int pageIndex, int totalPages, int pageWindow, RouteInfo routeInfo){
        PageIndex = pageIndex;
        TotalPages = totalPages;
        PageWindow = pageWindow;
        RouteInfo = routeInfo;
    }
}
