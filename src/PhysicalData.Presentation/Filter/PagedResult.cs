namespace PhysicalData.Presentation.Filter
{
    public sealed class PagedResult<T> where T : class
    {
        public required IEnumerable<T> Content { get; init; }
        public required int ResultCount { get; init; }
    }
}
