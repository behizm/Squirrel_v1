using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.ViewModels
{
    public class PagingModel
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public string PagingMethod { get; set; }
    }
}
