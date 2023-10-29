using Microsoft.AspNetCore.SignalR;
using StudentsRankingApiDemo.Models;

namespace StudentsRankingApiDemo.Hubs
{
    public class StudentRankingHub : Hub
    {
        public StudentRankingHub()
        {
        }

        public void SendUpdatedStudentsSummary(IEnumerable<StudentPointsSummary> list)
        {
            Clients.All.SendAsync("updateSummary", list);
        }
    }
}
