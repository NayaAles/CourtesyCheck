namespace CourtesyСheck
{
    public class ManagerByPeriod
    {
        public string ManagerEmail { get; set; } = null!;
        public int EmailsCountAll { get; set; }
        public int PoliteAll { get; set; }
    }

    public class Manager
    {
        public string ManagerEmail { get; set; } = null!;
    }

    public class Polite
    {
        public string Date { get; set; } = null!;
        public string ManagerEmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int EmailsCountAll { get; set; }
        public int PoliteAll { get; set; }
        public int Persent { get; set; }
    }

    public class Check
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? ManagerEmail { get; set; }
        public string? Text { get; set; }
        public string? TextNext { get; set; }
        public int? Marker { get; set; }
    }
}
