using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `Join` to wait until created Thread finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            var exceptionThrown = false;
            for (var i = 0; i < repeats; i++)
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        action();
                    }
                    catch (Exception e)
                    {
                        if (errorAction is not null)
                            errorAction(e);
                        exceptionThrown = true;
                    }
                })
                {
                    IsBackground = true
                };
                if (exceptionThrown)
                    return;
                thread.Start();
                thread.Join();
            }
        }

        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            // First attempt was using ThreadPool.RegisterWaitForSingleObject -> This worked because it ran into timeouts
            // (or alternatively with Thread.Sleep in the main thread)

            var exceptionThrown = false;
            for (var i = 0; i < repeats; i++)
            {
                var autoResetEvent = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(state=> {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        action();
                    }
                    catch (Exception e)
                    {
                        if (errorAction is not null)
                        {
                            exceptionThrown = true;
                            errorAction(e);
                        }
                    }
                    finally
                    {
                        autoResetEvent.Set();
                    }
                });
                autoResetEvent.WaitOne();
                autoResetEvent.Close();
                if (exceptionThrown)
                    break;
            }
        }
    }
}
