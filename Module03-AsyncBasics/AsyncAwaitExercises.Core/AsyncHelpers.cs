﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OperationCanceledException = System.OperationCanceledException;

namespace AsyncAwaitExercises.Core
{
    public class AsyncHelpers
    {
        public static Task<string> GetStringWithRetries(HttpClient client, string url, int maxTries = 3, CancellationToken token = default)
        {
            if (maxTries < 2)
                throw new ArgumentException($"{nameof(maxTries)} must be at least 2");

            return GetStringWithRetriesAsync(client, url, maxTries, token);
        }

        private static async Task<string> GetStringWithRetriesAsync(HttpClient client, string url, int maxTries, CancellationToken token)
        {
            // Create a method that will try to get a response from a given `url`, retrying `maxTries` number of times.
            // It should wait one second before the second try, and double the wait time before every successive retry
            // (so pauses before retries will be 1, 2, 4, 8, ... seconds).
            // * `maxTries` must be at least 2
            // * we retry if:
            //    * we get non-successful status code (outside of 200-299 range), or
            //    * HTTP call thrown an exception (like network connectivity or DNS issue)
            // * token should be able to cancel both HTTP call and the retry delay
            // * if all retries fails, the method should throw the exception of the last try
            // HINTS:
            // * `HttpClient.GetStringAsync` does not accept cancellation token (use `GetAsync` instead)
            // * you may use `EnsureSuccessStatusCode()` method
            
            var delay = 1000;
            var tries = 0;
            Exception lastException = null;

            while (tries < maxTries)
            {
                try
                {
                    var response = await client.GetAsync(url, token);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync(token);
                }
                catch (OperationCanceledException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    await Task.Delay(delay, token);
                    delay *= 2;
                    tries++;
                    lastException = e;
                }
            }

            throw lastException!;
        }

        private static ReadOnlySpan<byte> Test()
        {
            
            return data;
        }
    }
}
