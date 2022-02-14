using API_Gestionnaire_de_Vacataire.Models;
using API_Gestionnaire_de_Vacataire.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Gestionnaire_de_Vacataire.Controllers
{
    [Route("api/EmploiDeTemps")]
    [ApiController]
    public class EmploiDeTempsControllers : ControllerBase
    {

        private IEmploiDeTempsRepository _emploiDeTempsRepository;

        public EmploiDeTempsControllers(IEmploiDeTempsRepository emploiDeTempsRepository)
        {
            this._emploiDeTempsRepository=emploiDeTempsRepository;
        }

        // GET: api/<EmploiDeTemps>
        [HttpGet]
        public async Task<ActionResult> GetAllEmploiDeTemps()
        {
            try
            {
                return Ok(await _emploiDeTempsRepository.GetAllEmploiDeTemps());
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET api/<EmploiDeTemps>/5
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetEmploiDeTempsById(int id)
        {
            try
            {
                var result=await _emploiDeTempsRepository.GetEmploiDeTempsById(id);
                if (result==null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur l'or de l'extraction des données");
                throw;
            }
        }

        // POST api/<EmploiDeTemps>
        [HttpPost]
        public int CreateEmploDeTemp(EmploiDeTemps emploiDeTemps)
        {
            if (emploiDeTemps!=null)
            {
                 _emploiDeTempsRepository.CreateEmploiDeTemps(emploiDeTemps);
                return 1;
            }
            return 0;
        }

        // PUT api/<EmploiDeTemps/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmploiDeTemps>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
