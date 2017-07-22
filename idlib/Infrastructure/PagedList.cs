using System;
using System.Collections.Generic;

namespace Genesis.idlib.Infrastructure
{
    public class PagedList<T> where T: class, new()
    {
        public IList<T> ItemList { get; set;}
        public PagedInfo PagedListInfo { get; set;}

        public PagedList()
        {
            ItemList = new List<T>();
            PagedListInfo = new PagedInfo();
        }
    }
}