using System;

namespace userhub.Models
{
    public class PagerModel
    {
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
    }
}