using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using CrimeRecordsManager.Models;

namespace CrimeRecordsManager.Controllers
{
    public class PoliceOfficersController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/PoliceOfficers
        [EnableQuery(MaxTop = 50)]
        public IQueryable<PoliceOfficer> Get()
        {
            return db.PoliceOfficers;
        }

        // GET: api/PoliceOfficers/5
        [EnableQuery]
        public SingleResult<PoliceOfficer> Get([FromODataUri] int id)
        {
            IQueryable<PoliceOfficer> result = db.PoliceOfficers.Where(p => p.Id == id);

            return SingleResult.Create(result);
        }

        // POST: api/PoliceOfficers
        public async Task<IHttpActionResult> Post(PoliceOfficer policeOfficer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PoliceOfficers.Add(policeOfficer);
            await db.SaveChangesAsync();

            return Created(policeOfficer);
        }

        // PUT: api/PoliceOfficers/5
        public async Task<IHttpActionResult> Put([FromODataUri] int id, PoliceOfficer policeOfficer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != policeOfficer.Id)
            {
                return BadRequest();
            }

            db.Entry(policeOfficer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliceOfficerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(policeOfficer);
        }

        // PATCH: api/PoliceOfficers/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int id, Delta<PoliceOfficer> policeOfficer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.PoliceOfficers.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            policeOfficer.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliceOfficerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        // DELETE: api/PoliceOfficers/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int id)
        {
            var policeOfficer = await db.PoliceOfficers.FindAsync(id);
            if (policeOfficer == null)
            {
                return NotFound();
            }

            db.PoliceOfficers.Remove(policeOfficer);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PoliceOfficerExists(int id)
        {
            return db.PoliceOfficers.Any(e => e.Id == id);
        }
    }
}