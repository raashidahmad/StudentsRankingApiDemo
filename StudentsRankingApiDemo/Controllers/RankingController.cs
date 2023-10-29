using Microsoft.AspNetCore.Mvc;
using StudentsRankingApiDemo.Models;
using StudentsRankingApiDemo.Services;

namespace StudentsRankingApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        IDBService dbService;
        public RankingController(IDBService dataService)
        {
            dbService = dataService;
        }

        [HttpPost]
        public IActionResult UpdateStudentPoints([FromBody] StudentPointsSummary summary)
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
            return Ok("Success");
        }

        [HttpGet(Name = "GetLatestSummary")]
        public IActionResult GetLatestSummary()
        {

            return Ok();
        }
    }
}
