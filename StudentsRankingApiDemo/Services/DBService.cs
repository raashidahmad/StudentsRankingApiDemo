using StudentsRankingApiDemo.Models;
using System.Data.SqlClient;

namespace StudentsRankingApiDemo.Services
{
    public interface IDBService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        int UpdateStudentPoints(int studentId, string studentName, int points);

        IEnumerable<StudentPointsSummary> GetLatestSummary();
    }

    public class DBService : IDBService
    {
        SqlConnection connection;
        public DBService()
        {
            connection = new SqlConnection();
            connection.ConnectionString = "server=.;User ID=sa;Password=master002;Database=StudentRanking";
        }

        public int UpdateStudentPoints(int studentId, string studentName, int points)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                string selectQuery = "SELECT * FROM StudentPointsSummary WHERE StudentId = @StudentId";
                SqlParameter studentIdParam = new SqlParameter("@StudentId", studentId);
                command.Parameters.Add(studentIdParam);
                command.CommandText = selectQuery;
                SqlDataReader reader = command.ExecuteReader();
                int studentPoints = 0;
                int affectedRows = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        studentPoints += reader.GetInt32(1);
                    }
                    reader.Close();

                    string updateQuery = "UPDATE StudentPointsSummary SET Points = @Points WHERE StudentId = @StudentId";
                    command.CommandText = updateQuery;
                    SqlParameter pointParam = new SqlParameter("@Points", points);
                    command.Parameters.Add(pointParam);
                    affectedRows = command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    command.CommandText = "INSERT INTO StudentPointsSummary VALUES(@StudentId, @StudentName, @Points)";
                    SqlParameter nameParam = new SqlParameter("@StudentName", studentName);
                    command.Parameters.Add(nameParam);
                    SqlParameter pointParam = new SqlParameter("@Points", points);
                    command.Parameters.Add(pointParam);
                    affectedRows = command.ExecuteNonQuery();
                }
                connection.Close();
                return affectedRows;
            }
        }

        public IEnumerable<StudentPointsSummary> GetLatestSummary()
        {
            using (var command = new SqlCommand())
            {
                List<StudentPointsSummary> summary = new List<StudentPointsSummary>();
                command.Connection = connection;
                connection.Open();
                string selectQuery = "SELECT TOP 5 FROM StudentPointsSummary Order By Points ASC";
                command.CommandText = selectQuery;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        summary.Add(new StudentPointsSummary()
                        {
                            StudentId = reader.GetInt32(0),
                            Points = reader.GetInt32(1)
                        });
                    }
                    reader.Close();
                }
                return summary;
            }
        }
    }
}
