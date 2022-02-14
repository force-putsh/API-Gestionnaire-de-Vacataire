using System;

namespace API_Gestionnaire_de_Vacataire.Models
{
    public class InfoEmploisDeTemps
    {
        public string cour { get; set; }
        public string Enseignant { get; set; }
        public string? Date   { get; set; }
        public string? HeureDebut { get; set; }
        public string? HeureFin { get; set; }
    }
}
