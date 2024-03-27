namespace CovidSystem.Services
{
    // Interface for summary data service
    public interface ISummeryDataService
    {
        public IEnumerable<object> GetGraphData(List<Member> members);
        public int GetNotVaccinatedMembers(List<Member> members);
    }
    public class SummeryDataService : ISummeryDataService
    {
        // Method to get graph data for active patients
        public IEnumerable<object> GetGraphData(List<Member> members)
        {
            try
            {
                DateTime endDate = DateTime.UtcNow.Date; // Today's date
                DateTime startDate = endDate.AddMonths(-1).AddDays(1); // Start date is 1 month ago
                var activePatients = members
                    .Select(m => new
                    {
                        MemberId = m.MemberId,
                        PositiveResultDate = m.PositiveResultDate,
                        RecoveryDate = m.RecoveryDate
                    })
                    .ToList()
                    .Where(m =>
                        m.PositiveResultDate.HasValue &&
                        m.PositiveResultDate.Value.Date <= endDate &&
                        (!m.RecoveryDate.HasValue || m.RecoveryDate.Value.Date > endDate)
                    )
                    .SelectMany(m =>
                    {
                        var dates = new List<DateTime>();
                        DateTime currentDate = m.PositiveResultDate.Value.Date;
                        while (currentDate <= endDate && (!m.RecoveryDate.HasValue || currentDate < m.RecoveryDate.Value.Date))
                        {
                            dates.Add(currentDate);
                            currentDate = currentDate.AddDays(1);
                        }
                        return dates;
                    })
                    .GroupBy(date => date.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderBy(g => g.Date)
                    .ToList();
                return activePatients.Select(p => new { X = p.Date.Day, Y = p.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGraphData method: {ex.Message}");
                return Enumerable.Empty<object>();
            }
        }
        // Method to get count of not vaccinated members
        public int GetNotVaccinatedMembers(List<Member> members)
        {
            try
            {
                var NotVaccinatedMembers = members
                    .ToList()
                    .Where(m => m.Vaccinations.Count() == 0);
                return NotVaccinatedMembers.Count();
            }
            catch (Exception ex)
            {
                // Print the error message directly
                Console.WriteLine($"Error in GetNotVaccinatedMembers method: {ex.Message}");
                return 0;
            }
        }
    }
}
