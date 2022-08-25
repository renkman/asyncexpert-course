using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        private readonly Dictionary<ulong,ulong> _cache = new();

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n <= 1)
                return n;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if(_cache.ContainsKey(n))
                return _cache[n];

            if (n <= 1)
            {
                _cache.Add(n, n);
                return n;
            }

            var result = RecursiveWithMemoization(n - 2) + RecursiveWithMemoization(n - 1);
            _cache.Add(n, result);

            return result;
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n <= 1)
                return n;

            ulong result = 0;
            ulong lastResult = 1;
            for (ulong i = 0; i < n; i++)
            {
                var preLastResult = lastResult;
                lastResult = result;

                result = preLastResult + lastResult;
            }
            return result;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
