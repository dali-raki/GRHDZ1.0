namespace GrhDz.Domains.Models.Employees
{
    public class Employe
    {
        public int EmployeID { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string NSecuriteSocial { get; set; }
        public string Adresse { get; set; }
        public string NTelephone { get; set; }
        public string SituationFamiliale { get; set; } 
        public string GroupSanguin { get; set; }
        public int FonctionID { get; set; }
        public DateTime DateEntree { get; set; }
        public DateTime? DateSortie { get; set; }
        public byte[] Photo { get; set; }
        public int Journee { get; set; }
        public string FonctionName { get; set; }
        public string Status { get; set; }
        public string FullName => $"{Nom} {Prenom}";
    }


}
