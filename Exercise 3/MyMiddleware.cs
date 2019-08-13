using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Exercise_4
{
    public class MyMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public MyMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }
        public async Task Invoke(IDictionary<string, object> ctx)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // Console.WriteLine("From class: Before");
            await _next(ctx);
            stopwatch.Stop();
            // Console.WriteLine($"From class: after. Execution time {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}