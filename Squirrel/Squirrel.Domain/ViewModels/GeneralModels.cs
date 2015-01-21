using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.ViewModels
{
    public class OrderingModel<T> where T : class
    {
        public OrderingModel()
        {
            IsAscending = true;
            Skip = 0;
            Take = 10;
        }

        public System.Linq.Expressions.Expression<Func<T, string>> KeySelector { get; set; }
        public bool IsAscending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class TaskDictionary<T> where T : class
    {
        public string Name { get; set; }
        public Task<T> Task { get; set; }
    }
}
