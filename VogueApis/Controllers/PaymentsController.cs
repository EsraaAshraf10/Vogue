using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using VogueApis.DTOs;
using VogueApis.Errors;
using VogueCore.Entities;
using VogueCore.Services;

namespace VogueApis.Controllers
{
    
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentServices;
        private readonly IMapper _mapper;

        const string endpointSecret = "whsec_d599a9cfab207eb078a62607386b66f1fed3ea3de454f779b47ffd6c7acba08e";

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentServices = paymentService;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There is a Problem With Your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(CustomerBasket);
            return Ok(MappedBasket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    await _paymentServices.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    await _paymentServices.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);

                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
