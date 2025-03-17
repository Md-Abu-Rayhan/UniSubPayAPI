using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CheckoutResponseDto>> CreateCheckout(CheckoutRequestDto request)
        {
            try
            {
                if (request.TournamentIds == null || request.TournamentIds.Count == 0)
                {
                    return BadRequest("No tournaments selected for checkout");
                }

                var response = await _checkoutService.CreateCheckoutAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("callback")]
        public async Task<ActionResult> ProcessCallback(PaymentCallbackDto callback)
        {
            try
            {
                if (string.IsNullOrEmpty(callback.TransactionId))
                {
                    return BadRequest("Transaction ID is required");
                }

                var result = await _checkoutService.ProcessPaymentCallbackAsync(callback);
                if (!result)
                {
                    return NotFound("Transaction not found");
                }

                return Ok(new { Success = true, Message = "Payment processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment callback");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}