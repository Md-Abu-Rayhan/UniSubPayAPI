using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ITournamentService
    {
        Task<List<Tournament>> GetAllTournamentsAsync();
        Task<Tournament> GetTournamentByIdAsync(int tournamentId);
        Task<List<Tournament>> GetTournamentsByIdsAsync(List<int> ids);
        Task<Tournament> CreateTournamentAsync(Tournament tournament);
        Task<Tournament> UpdateTournamentAsync(Tournament tournament);
        Task<bool> DeleteTournamentAsync(int tournamentId);
    }
}