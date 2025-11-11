namespace GrhDz.Domains.Models.Remboursements
{
    public class RemboursementType
    {
        public int RemboursementID { get; set; }
        public int EmployeID { get; set; }
        public decimal Montant { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
