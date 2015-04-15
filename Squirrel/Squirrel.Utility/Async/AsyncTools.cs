using System;
using System.Threading.Tasks;

namespace Squirrel.Utility.Async
{
    public class AsyncTools
    {
        public static T ConvertToSync<T>(Func<Task<T>> func)
        {
            var task = Task.Run(func);
            task.Wait();
            return task.Result;
        }

        public static void ConvertToSync(Func<Task> func)
        {
            var task = Task.Run(func);
            task.Wait();
        }
    }
}
