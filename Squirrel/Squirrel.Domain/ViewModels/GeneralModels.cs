using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Squirrel.Domain.ViewModels
{
    public class OrderingModel<TSource, TKey> where TSource : class
    {
        public OrderingModel()
        {
            IsAscending = true;
            Skip = 0;
            Take = 10;
        }

        public System.Linq.Expressions.Expression<Func<TSource, TKey>> OrderByKeySelector { get; set; }
        public Func<TSource, TKey> OrderByKeySelectorFunc { get; set; }
        public bool IsAscending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class OrderingModel<TSource> : OrderingModel<TSource, string> where TSource : class
    {
    }

    public class TaskDictionary<T> where T : class
    {
        public string Name { get; set; }
        public Task<T> Task { get; set; }
    }

    public class ListModel<T>
    {
        public List<T> List { get; set; }
        public int CountOfAll { get; set; }
    }
}
