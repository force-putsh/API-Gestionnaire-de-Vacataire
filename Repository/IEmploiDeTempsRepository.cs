using API_Gestionnaire_de_Vacataire.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Gestionnaire_de_Vacataire.Repository
{
    public interface IEmploiDeTempsRepository
    {
        Task<IEnumerable<InfoEmploisDeTemps>> GetAllEmploiDeTemps();
        Task<IEnumerable<InfoEmploisDeTemps>> GetEmploiDeTempsById(int Id);
        Task<InfoEmploisDeTemps> CreateEmploiDeTemp(InfoEmploisDeTemps emploisDeTemps);
        Task<InfoEmploisDeTemps> TcheckEmploiDeTemps(string Nom, string matiere);
    }
}
