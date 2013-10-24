using System.Collections.Generic;

namespace Web.SPA.Models
{
    public class PageResult
    {
        public int Total { get; set; }

        public IEnumerable<object> Data { get; set; }
    }
}