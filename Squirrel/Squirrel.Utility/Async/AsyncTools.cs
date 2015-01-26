using System;
using System.Threading.Tasks;

namespace Squirrel.Utility.Async
{
    public class AsyncTools
    {
        public static T ConvertToSync<T>(Func<Task<T>> func) where T : class
        {
            var task = Task.Run(func);
            task.Wait();
            return task.Result;
        }
    }
}
