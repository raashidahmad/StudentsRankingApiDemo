using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentsRankingApiDemo.Hubs;
using StudentsRankingApiDemo.Models;
using StudentsRankingApiDemo.Services;

namespace StudentsRankingApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        IDBService dbService;
        private readonly IHubContext<StudentRankingHub> hubContext;

        public RankingController(IDBService dataService, IHubContext<StudentRankingHub> rankingHub)
        {
            dbService = dataService;
            hubContext = rankingHub;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudentPoints([FromBody] StudentPointsSummary summary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (summary.StudentId <= 0)
            {
                return BadRequest("Invalid Student Id provided");
            }

            int updated = dbService.UpdateStudentPoints(summary.StudentId, summary.StudentName, summary.Points);
            if (updated <= 0)
            {
                return BadRequest("An error occured");
            }

            var latestSummary = dbService.GetLatestSummary();
            await hubContext.Clients.All.SendAsync("summaryUpdated", latestSummary);
            return Ok("Success");
        }

        [HttpGet(Name = "GetLatestSummary")]
        public IActionResult GetLatestSummary()
        {
            return Ok(dbService.GetLatestSummary());
        }
    }
}
