namespace Utils
{
    public struct BenchmarkResult<T>
    {
        public string Name { get; set; }

        public double TotalSeconds { get; set; }

        public T Result { get; set; }
    }
}
