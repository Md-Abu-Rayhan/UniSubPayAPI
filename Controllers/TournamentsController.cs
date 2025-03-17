using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly ILogger<TournamentsController> _logger;

        public TournamentsController(ITournamentService tournamentService, ILogger<TournamentsController> logger)
        {
            _tournamentService = tournamentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<Tournament>>>> GetAllTournaments()
        {
            try
            {
                var tournaments = await _tournamentService.GetAllTournamentsAsync();
                return Ok(ApiResponse<List<Tournament>>.SuccessResponse(tournaments, "Tournaments retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tournaments");
                return StatusCode(500, ApiResponse<List<Tournament>>.ErrorResponse("An error occurred while retrieving tournaments"));
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ApiResponse<Tournament>>> GetTournament(int id)
        {
            try
            {
                var tournament = await _tournamentService.GetTournamentByIdAsync(id);
                if (tournament.Id == 0)
                {
                    return NotFound(ApiResponse<Tournament>.ErrorResponse($"Tournament with ID {id} not found"));
                }
                return Ok(ApiResponse<Tournament>.SuccessResponse(tournament, "Tournament retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tournament with ID {Id}", id);
                return StatusCode(500, ApiResponse<Tournament>.ErrorResponse("An error occurred while retrieving the tournament"));
            }
        }

        [HttpPost("getByIds")]
        public async Task<ActionResult<ApiResponse<List<Tournament>>>> GetTournamentsByIds([FromBody] TournamentListRequestDto request)
        {
            try
            {
                if (request?.TournamentIds == null || request.TournamentIds.Count == 0)
                {
                    return BadRequest(ApiResponse<List<Tournament>>.ErrorResponse("No tournament IDs provided"));
                }

                var tournaments = await _tournamentService.GetTournamentsByIdsAsync(request.TournamentIds);
                return Ok(ApiResponse<List<Tournament>>.SuccessResponse(tournaments, "Tournaments retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tournaments by IDs");
                return StatusCode(500, ApiResponse<List<Tournament>>.ErrorResponse("An error occurred while retrieving tournaments"));
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<Tournament>>> CreateTournament([FromBody] Tournament tournament)
        {
            try
            {
                if (tournament == null)
                {
                    return BadRequest(ApiResponse<Tournament>.ErrorResponse("Tournament data is required"));
                }

                var createdTournament = await _tournamentService.CreateTournamentAsync(tournament);
                return Ok(ApiResponse<Tournament>.SuccessResponse(createdTournament, "Tournament created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tournament");
                return StatusCode(500, ApiResponse<Tournament>.ErrorResponse("An error occurred while creating the tournament"));
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiResponse<Tournament>>> UpdateTournament([FromBody] Tournament tournament)
        {
            try
            {
                if (tournament == null)
                {
                    return BadRequest(ApiResponse<Tournament>.ErrorResponse("Tournament data is required"));
                }

                var updatedTournament = await _tournamentService.UpdateTournamentAsync(tournament);
                if (updatedTournament.Id == 0)
                {
                    return NotFound(ApiResponse<Tournament>.ErrorResponse($"Tournament with ID {tournament.Id} not found"));
                }
                
                return Ok(ApiResponse<Tournament>.SuccessResponse(updatedTournament, "Tournament updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tournament with ID {Id}", tournament?.Id);
                return StatusCode(500, ApiResponse<Tournament>.ErrorResponse("An error occurred while updating the tournament"));
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTournament(int id)
        {
            try
            {
                var result = await _tournamentService.DeleteTournamentAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<bool>.ErrorResponse($"Tournament with ID {id} not found"));
                }
                
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Tournament deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tournament with ID {Id}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the tournament"));
            }
        }
    }
}