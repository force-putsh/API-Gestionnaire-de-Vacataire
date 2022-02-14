using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_Gestionnaire_de_Vacataire.Models
{
    public partial class Contrat
    {
        public int Id { get; set; }
        public string NomCours { get; set; }
        public string NomVacataire { get; set; }
        public int? IdVacataire { get; set; }
        public int? Duree { get; set; }
        public decimal? SalaireHoraire { get; set; }

        public virtual Vacataire IdVacataireNavigation { get; set; }
    }
}
