using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Proect.Core
{
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize > 100 ? 100 : pageSize;
            this.TotalCount = source.Count();
            this.TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                this.TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Data = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            pageSize = pageSize < 1 ? 1 : pageSize > 1 ? 1 : pageSize;
            this.TotalCount = source.Count();
            this.TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                this.TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Data = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
           // this.AddRange();
        }

        public bool HasNextPage
        {
            get { return (PageIndex + 1 <= TotalPages); }
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }
        public IList<T>  Data
        {
            get;
        }
        public int PageIndex
        {
            get;
        }

        public int PageSize
        {
            get;
        }

        public int TotalCount
        {
            get;
        }

        public int TotalPages
        {
            get;
        }
    }
}
