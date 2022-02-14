using API_Gestionnaire_de_Vacataire.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Gestionnaire_de_Vacataire.Repository
{
    public class EmploiDeTempsRepository:IEmploiDeTempsRepository
    {
        readonly DbGestionnaireStagiaireContext _dbContext;
        public EmploiDeTempsRepository(DbGestionnaireStagiaireContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<InfoEmploisDeTemps>> GetAllEmploiDeTemps()
        {
            var result =await (from E in _dbContext.EmploiDeTemps
                         join V in _dbContext.Vacataire on E.IdVacataire equals V.Id
                               select new InfoEmploisDeTemps
                         {
                             cour=E.NomCours,
                             Enseignant=V.Nom,
                             Date=E.Date.ToString(),
                             HeureDebut=E.HeureDebut.ToString(),
                             HeureFin=E.HeureFin.ToString(),

                         }).ToListAsync();
            return result;
        }


        public async Task<IEnumerable<InfoEmploisDeTemps>> GetEmploiDeTempsById(int Id)
        {
            var result =await  (from E in _dbContext.EmploiDeTemps
                                join V in _dbContext.Vacataire on E.IdVacataire equals V.Id
                                where V.Id == Id
                                select new InfoEmploisDeTemps
                                {
                                    cour = E.NomCours,
                                    Enseignant = V.Nom,
                                    Date = E.Date.ToString(),
                                    HeureDebut = E.HeureDebut.ToString(),
                                    HeureFin = E.HeureFin.ToString(),

                                }).ToListAsync();
            return result;
        }

        public async Task<InfoEmploisDeTemps> TcheckEmploiDeTemps(string Nom, string matiere)
        {
            var result = await (from E in _dbContext.EmploiDeTemps
                                join V in _dbContext.Vacataire on E.IdVacataire equals V.Id
                                where E.NomCours == matiere && V.Nom == Nom
                                select new InfoEmploisDeTemps
                                {
                                    cour = E.NomCours,
                                    Enseignant = V.Nom,
                                    Date = E.Date.ToString(),
                                    HeureDebut = E.HeureDebut.ToString(),
                                    HeureFin = E.HeureFin.ToString(),

                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}
