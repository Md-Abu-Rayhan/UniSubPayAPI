using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Configurations;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ITournamentService _tournamentService;
        private readonly IUserService _userService;
        private readonly BkashSettings _bkashSettings;
        
        private readonly List<TournamentRegistration> _registrations = new List<TournamentRegistration>();
        private readonly List<Payment> _payments = new List<Payment>();

        public CheckoutService(
            ITournamentService tournamentService, 
            IUserService userService,
            IOptions<BkashSettings> bkashSettings)
        {
            _tournamentService = tournamentService;
            _userService = userService;
            _bkashSettings = bkashSettings.Value;
        }

        public async Task<CheckoutResponseDto> CreateCheckoutAsync(CheckoutRequestDto request)
        {

            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var tournaments = await _tournamentService.GetTournamentsByIdsAsync(request.TournamentIds);
            if (tournaments.Count == 0)
            {
                throw new Exception("No valid tournaments found");
            }

            decimal totalAmount = tournaments.Sum(t => t.EntryFee);
            string transactionId = Guid.NewGuid().ToString("N");
            foreach (var tournament in tournaments)
            {
                _registrations.Add(new TournamentRegistration
                {
                    Id = _registrations.Count + 1,
                    UserId = user.Id,
                    TournamentId = tournament.Id,
                    RegistrationDate = DateTime.UtcNow,
                    PaymentStatus = "Pending",
                    TransactionId = transactionId
                });
            }
            var checkoutUrl = $"{_bkashSettings.CheckoutUrl}?transactionId={transactionId}&amount={totalAmount}&returnUrl={request.ReturnUrl}&cancelUrl={request.CancelUrl}";

            return new CheckoutResponseDto
            {
                CheckoutUrl = checkoutUrl,
                TransactionId = transactionId,
                TotalAmount = totalAmount,
                Status = "Pending"
            };
        }

        public async Task<bool> ProcessPaymentCallbackAsync(PaymentCallbackDto callback)
        {
            await Task.Delay(1);             
            var registrations = _registrations.Where(r => r.TransactionId == callback.TransactionId).ToList();
            if (registrations.Count == 0)
            {
                return false;
            }

            foreach (var registration in registrations)
            {
                registration.PaymentStatus = callback.Status;
            }
            _payments.Add(new Payment
            {
                Id = _payments.Count + 1,
                TransactionId = callback.TransactionId,
                Amount = callback.Amount,
                Currency = callback.Currency,
                Status = callback.Status,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = callback.PaymentMethod,
                UserId = registrations.First().UserId
            });

            return true;
        }

        public async Task<SubscriptionResponseDto> CreateSubscriptionAsync(SubscriptionRequestDto request)
        {

            var user = await _userService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var tournament = await _tournamentService.GetTournamentByIdAsync(request.TournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament not found");
            }

            if (string.IsNullOrEmpty(request.SubscriptionRequestId))
            {
                request.SubscriptionRequestId = $"SUB-{Guid.NewGuid().ToString("N")}";
            }
            if (string.IsNullOrEmpty(request.Frequency))
            {
                throw new Exception("Frequency is required");
            }

            _registrations.Add(new TournamentRegistration
            {
                Id = _registrations.Count + 1,
                UserId = user.Id,
                TournamentId = tournament.Id,
                RegistrationDate = DateTime.UtcNow,
                PaymentStatus = "Pending",
                TransactionId = request.SubscriptionRequestId
            });
            var paymentUrl = $"{_bkashSettings.CheckoutUrl}?subscriptionRequestId={request.SubscriptionRequestId}&amount={request.Amount}&redirectUrl={request.RedirectUrl}";

            return new SubscriptionResponseDto
            {
                PaymentUrl = paymentUrl,
                SubscriptionRequestId = request.SubscriptionRequestId,
                Status = "Pending",
                Frequency = request.Frequency
            };
        }
    }
}