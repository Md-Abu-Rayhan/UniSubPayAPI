using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResponseDto> CreateCheckoutAsync(CheckoutRequestDto request);
        Task<bool> ProcessPaymentCallbackAsync(PaymentCallbackDto callback);
        Task<SubscriptionResponseDto> CreateSubscriptionAsync(SubscriptionRequestDto request);
    }
}