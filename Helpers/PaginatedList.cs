using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MasterCore8.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        // public int CurrentPage { get; private set; }
        // public int TotalPages { get; private set; }
        // public int RowCount { get; private set; }
        
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }

        public PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize = 10, int maxPages = 5)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages) 
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages-1;
            }
            else 
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage) 
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                } 
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages) 
                {
                    // current page near the end
                    startPage = totalPages - maxPages;
                    endPage = totalPages-1;
                }
                else 
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex > 0 ? startIndex : 0;
            EndIndex = endIndex;

            this.AddRange(items);
        }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize, int maxPages = 10)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, currentPage, pageSize, maxPages);
        }

        public string GenPagination(string uri=""){
            if(TotalPages < 2) return "";
            using (var html = new System.IO.StringWriter())
            {
                int PreviousPage = (CurrentPage-1) < 1 ? 1 : (CurrentPage-1);
                int NexPage = (CurrentPage+1) > TotalPages ? TotalPages : (CurrentPage+1);
                html.Write(@$"
                     <nav aria-label='Page navigation'>
                        <ul class='pagination custom-pagination my-0'>
                            <li class='page-item {(HasPreviousPage ? "" : "disabled")}'>
                            <a class='page-link' href='{GenUrlPageLink(uri, PreviousPage.ToString())}' aria-label='Previous'>
                                &laquo;
                            </a>
                            </li>
                            <li class='page-item {(CurrentPage == 1 ? "active" : "")}'><a class='page-link' href='{GenUrlPageLink(uri, "1")}'>1</a></li>
                ");

                if(StartPage > 2){
                    html.Write(@$"
                                <li class='page-item disabled'><a class='page-link'> <span aria-hidden='true'>...</span></a></li>
                    ");
                }

                for (int i = StartPage; i < EndPage+1; i++)
                {
                    if(i <= 1 || i > TotalPages) continue; 
                    
                    html.Write(@$"
                                <li class='page-item {(CurrentPage ==i ? "active" : "")}'><a class='page-link' href='{GenUrlPageLink(uri, i.ToString())}'>{i}</a></li>
                    ");

                }

                if(EndPage < TotalPages-1){
                    html.Write(@$"
                                <li class='page-item disabled'><a class='page-link'> <span aria-hidden='true'>...</span></a></li>
                    ");
                }
                 
                

                html.Write(@$"
                            <li class='page-item {(CurrentPage == TotalPages ? "active" : "")}'><a class='page-link' href='{GenUrlPageLink(uri, TotalPages.ToString())}'>{TotalPages}</a></li>
                            <li class='page-item {(HasNextPage ? "" : "disabled")}'>
                            <a class='page-link' href='{GenUrlPageLink(uri, NexPage.ToString())}' aria-label='Next'>
                                &raquo;
                            </a>
                            </li>
                        </ul>
                    </nav>
                ");

                return html.ToString();
            }

        }

        public string GenUrlPageLink(string uri="",string page="1"){
            if(string.IsNullOrEmpty(uri)){
                return "?page="+page;
            }

            var uriBuilder = new UriBuilder(uri);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["page"] = page;
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}