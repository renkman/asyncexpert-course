using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
    public class AsyncTools
    {
        public static Task<string> RunProgramAsync(string path, string args = "")
        {
            var programTaskCompletionSource = new TaskCompletionSource<string>();

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = path;
            processStartInfo.Arguments = args;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;


            var process = Process.Start(processStartInfo);
            if (process is null)
            {
                programTaskCompletionSource.SetCanceled();
                return programTaskCompletionSource.Task;
            }

            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                programTaskCompletionSource.TrySetResult(process.StandardOutput.ReadToEnd());
                return programTaskCompletionSource.Task;
            }

            var exception = new Exception(process.StandardError.ReadToEnd());
            programTaskCompletionSource.TrySetException(exception);
            return programTaskCompletionSource.Task;
        }
    }
}
