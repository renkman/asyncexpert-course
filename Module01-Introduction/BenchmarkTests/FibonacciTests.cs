using Benchmark;

namespace BenchmarkTests
{
    public class FibonacciTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(6, 8)]
        [InlineData(8, 21)]
        [InlineData(15, 610)]
        public void Recursive_WithSteps_ReturnsExpectedResult(ulong n, ulong expected)
        {
            var calc = new FibonacciCalc();
            var result = calc.Recursive(n);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(6, 8)]
        [InlineData(8, 21)]
        [InlineData(15, 610)]
        public void Iterative_WithSteps_ReturnsExpectedResult(ulong n, ulong expected)
        {
            var calc = new FibonacciCalc();
            var result = calc.Iterative(n);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(6, 8)]
        [InlineData(8, 21)]
        [InlineData(15, 610)]

        public void RecursiveWithMemoization_WithSteps_ReturnsExpectedResult(ulong n, ulong expected)
        {
            var calc = new FibonacciCalc();
            var result = calc.RecursiveWithMemoization(n);

            Assert.Equal(expected, result);
        }
    }
}