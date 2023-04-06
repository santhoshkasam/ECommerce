using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;

        public SearchService(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            if (orderResult.IsSuccess)
            {
                var result = new { Orders = orderResult.Orders };
                return (true, result);
            }
            return (false, null);
        }
    }
}
