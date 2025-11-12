namespace GrhDz.Domains.Models.Dashboards
{
    public class DashboardModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Dette { get; set; } = 0;
        public decimal Avance { get; set; } = 0;
    }
}
