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
    public class SubscriptionController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(ICheckoutService checkoutService, ILogger<SubscriptionController> logger)
        {
            _checkoutService = checkoutService;
            _logger = logger;
        }

        [HttpPost("create/weekly")]
        public async Task<ActionResult<SubscriptionResponseDto>> CreateWeeklySubscription([FromBody] SubscriptionRequestDto request)
        {
            try
            {
                // Set weekly subscription parameters
                request.Frequency = "WEEKLY";
                request.Amount = request.Amount > 0 ? request.Amount : 1; // Default to 1 BDT if not specified
                
                if (request.StartDate == DateTime.MinValue)
                    request.StartDate = DateTime.UtcNow;
                
                if (request.ExpiryDate == DateTime.MinValue)
                    request.ExpiryDate = request.StartDate.AddMonths(3); // 3 months subscription
                
                // Generate unique subscription request ID if not provided
                if (string.IsNullOrEmpty(request.SubscriptionRequestId))
                    request.SubscriptionRequestId = $"WEEKLY-{Guid.NewGuid().ToString("N")}";
                
                var response = await _checkoutService.CreateSubscriptionAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating weekly subscription");
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpPost("create/monthly")]
        public async Task<ActionResult<SubscriptionResponseDto>> CreateMonthlySubscription([FromBody] SubscriptionRequestDto request)
        {
            try
            {
                // Set monthly subscription parameters
                request.Frequency = "MONTHLY";
                request.Amount = request.Amount > 0 ? request.Amount : 1; // Default to 1 BDT if not specified
                
                if (request.StartDate == DateTime.MinValue)
                    request.StartDate = DateTime.UtcNow;
                
                if (request.ExpiryDate == DateTime.MinValue)
                    request.ExpiryDate = request.StartDate.AddMonths(6); // 6 months subscription
                
                // Generate unique subscription request ID if not provided
                if (string.IsNullOrEmpty(request.SubscriptionRequestId))
                    request.SubscriptionRequestId = $"MONTHLY-{Guid.NewGuid().ToString("N")}";
                
                var response = await _checkoutService.CreateSubscriptionAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating monthly subscription");
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }
    }
}