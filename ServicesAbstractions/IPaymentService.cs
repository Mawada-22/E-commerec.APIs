using Shared.DTOs;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IPaymentService
    {
        // Creates (or updates, if the basket already has one) the Stripe
        // PaymentIntent for this basket and returns the basket carrying the
        // ClientSecret the frontend confirms the card payment with.
        public Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);

        // Called by the Stripe webhook: flips the matching order's status to
        // PaymentReceived / PaymentFailed.
        public Task UpdateOrderPaymentStatusAsync(string paymentIntentId, bool succeeded);
    }
}
