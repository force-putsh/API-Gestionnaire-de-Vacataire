using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_Gestionnaire_de_Vacataire.Models
{
    public partial class Payement
    {
        public int Id { get; set; }
        public decimal? SalaireActuel { get; set; }
        public decimal? SalairePrevisionel { get; set; }

        public virtual Vacataire IdNavigation { get; set; }
    }
}
