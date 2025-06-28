using ComillaCentralMedical.Models;
using ComillaCentralMedical.Context;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ComillaCentralMedical.Controllers.API
{
    public class ServiceApiController : ApiController
    {
        private readonly MedicalDbContext db = new MedicalDbContext();

        [HttpGet]
        public IEnumerable<Service> GetAllServices()
        {
            return db.Services.Where(s => s.IsAvailable).ToList();
        }
    }
}
