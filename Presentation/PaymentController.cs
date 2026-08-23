using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServicesAbstractions;
using Shared.DTOs;
using Stripe;
using System.IO;
using System.Threading.Tasks;

namespace Presentation
{
    [Route("payments")]
    public class PaymentController(IServiceManager _serviceManager, IConfiguration _configuration) : ApiController
    {
        // Creates/updates the Stripe PaymentIntent for the basket and returns the
        // basket carrying the ClientSecret the frontend confirms the payment with.
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(basket);
        }

        // Stripe calls this endpoint - authentication is the signature header, not a JWT.
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var webhookSecret = _configuration["StripeSettings:WebhookSecret"];
            var stripeEvent = EventUtility.ConstructEvent(
                json, Request.Headers["Stripe-Signature"], webhookSecret);

            if (stripeEvent.Data.Object is PaymentIntent intent)
            {
                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        await _serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(intent.Id, succeeded: true);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        await _serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(intent.Id, succeeded: false);
                        break;
                }
            }

            // Always 200 so Stripe doesn't keep retrying events we deliberately ignore.
            return Ok();
        }
    }
}
