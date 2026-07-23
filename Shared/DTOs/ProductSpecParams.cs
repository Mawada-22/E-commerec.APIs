namespace Shared.DTOs
{
    public enum ProductSortingOptions
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }
    public class ProductSpecParams
    {
        // Filtering
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }

        // Sorting - string values sent by the storefront client:
        // "name" (default, A-Z), "nameDesc", "priceAsc", "priceDesc".
        public string? Sort { get; set; }

        // Paging
        public int PageIndex { get; set; } = 1;

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;

        private int _pageSize = DefaultPageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
