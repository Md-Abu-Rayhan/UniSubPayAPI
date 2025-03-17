using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class TournamentService : ITournamentService
    {
        // In a real application, this would come from a database
        private readonly List<Tournament> _tournaments = new List<Tournament>
        {
            new Tournament
            {
                Id = 1,
                Name = "Summer Chess Tournament",
                Description = "Annual summer chess tournament for all skill levels",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(32),
                EntryFee = 50.00m,
                MaxParticipants = 64,
                IsActive = true,
                Location = "Dhaka Chess Club",
                OrganizerId = "admin"
            },
            new Tournament
            {
                Id = 2,
                Name = "Weekly Poker Night",
                Description = "Weekly poker tournament with cash prizes",
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(7),
                EntryFee = 100.00m,
                MaxParticipants = 10,
                IsActive = true,
                Location = "Card Room, Gulshan",
                OrganizerId = "admin"
            },
            new Tournament
            {
                Id = 3,
                Name = "Badminton Championship",
                Description = "Annual badminton championship for singles and doubles",
                StartDate = DateTime.Now.AddDays(45),
                EndDate = DateTime.Now.AddDays(47),
                EntryFee = 75.00m,
                MaxParticipants = 32,
                IsActive = true,
                Location = "National Sports Complex",
                OrganizerId = "admin"
            }
        };

        public Task<List<Tournament>> GetAllTournamentsAsync()
        {
            return Task.FromResult(_tournaments.Where(t => t.IsActive).ToList());
        }

        public Task<Tournament> GetTournamentByIdAsync(int tournamentId)
        {
            var tournament = _tournaments.FirstOrDefault(t => t.Id == tournamentId);
            
            if (tournament == null)
            {
                tournament = new Tournament { Id = 0 };
            }
            
            return Task.FromResult(tournament);
        }

        public Task<List<Tournament>> GetTournamentsByIdsAsync(List<int> ids)
        {
            return Task.FromResult(_tournaments.Where(t => ids.Contains(t.Id) && t.IsActive).ToList());
        }

        public Task<Tournament> CreateTournamentAsync(Tournament tournament)
        {
            tournament.Id = _tournaments.Max(t => t.Id) + 1;
            
            _tournaments.Add(tournament);
            return Task.FromResult(tournament);
        }

        public Task<Tournament> UpdateTournamentAsync(Tournament tournament)
        {
            var existingTournament = _tournaments.FirstOrDefault(t => t.Id == tournament.Id);
            if (existingTournament == null)
            {
                return Task.FromResult(new Tournament { Id = 0 });
            }

            existingTournament.Name = tournament.Name;
            existingTournament.Description = tournament.Description;
            existingTournament.StartDate = tournament.StartDate;
            existingTournament.EndDate = tournament.EndDate;
            existingTournament.EntryFee = tournament.EntryFee;
            existingTournament.MaxParticipants = tournament.MaxParticipants;
            existingTournament.IsActive = tournament.IsActive;
            existingTournament.Location = tournament.Location;
            
            return Task.FromResult(existingTournament);
        }

        public Task<bool> DeleteTournamentAsync(int tournamentId)
        {
            var tournament = _tournaments.FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return Task.FromResult(false);
            }

            // Soft delete - just mark as inactive
            tournament.IsActive = false;
            return Task.FromResult(true);
        }
    }
}