using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_Gestionnaire_de_Vacataire.Models
{
    public partial class Vacataire
    {
        public Vacataire()
        {
            Contrat = new HashSet<Contrat>();
            EmploiDeTemps = new HashSet<EmploiDeTemps>();
            Pointage = new HashSet<Pointage>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string NumTel { get; set; }
        public string Password { get; set; }

        public virtual Payement Payement { get; set; }
        public virtual ICollection<Contrat> Contrat { get; set; }
        public virtual ICollection<EmploiDeTemps> EmploiDeTemps { get; set; }
        public virtual ICollection<Pointage> Pointage { get; set; }
    }
}
