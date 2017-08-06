using System;

namespace userhub.Models
{
    public class PagerModel
    {
        private int _maxDisplayPages;
        public PagerModel(string baseUrl,
                          int currentPage,
                          int sortBy,
                          string sortParam,
                          int totalPages,
                          int maxDisplayPages = 5)
        {
            BaseTargetUrl = baseUrl;
            CurrentPage = currentPage;
            SortBy = sortBy;
            SortParam = sortParam;
            TotalPages = totalPages;
            _maxDisplayPages = maxDisplayPages;
        }
        public int TotalPages { get; set; }
        
        public int CurrentPage { get; set; }

        public int SortBy { get; set; }

        public string SortParam { get; set;}

        public string BaseTargetUrl { get; set; }

        public bool PrevPageEnabled {
            get{
                return !(CurrentPage == 1);
            }
        }

        public bool NextPageEnabled {
            get{
                return !(CurrentPage == TotalPages);
            }
        }

        public int MaxDisplayPages {
            get{
                return  _maxDisplayPages > TotalPages ? TotalPages : _maxDisplayPages;
            }
        }

        public int UpperLimit {
            get{
                return (StartPagerAtPage  + MaxDisplayPages - 1);
            }
        }

        public int StartPagerAtPage {
            get{
                return (CurrentPage) > MaxDisplayPages ? (CurrentPage - MaxDisplayPages) + 1 : 1;
            }
        }

    }
}