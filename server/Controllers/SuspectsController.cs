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
    public class SuspectsController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/Suspects
        [EnableQuery(MaxTop = 50)]
        public IQueryable<Suspect> Get()
        {
            return db.Suspects;
        }

        // GET: api/Suspects/5
        [EnableQuery]
        public SingleResult<Suspect> Get([FromODataUri] int key)
        {
            IQueryable<Suspect> result = db.Suspects.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }

        // POST: api/Suspects
        public async Task<IHttpActionResult> Post(Suspect Suspect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suspects.Add(Suspect);
            await db.SaveChangesAsync();

            return Created(Suspect);
        }

        // PUT: api/Suspects/5
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Suspect Suspect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != Suspect.Id)
            {
                return BadRequest();
            }

            db.Entry(Suspect).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuspectExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Suspect);
        }

        // PATCH: api/Suspects/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Suspect> Suspect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Suspects.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            Suspect.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuspectExists(key))
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

        // DELETE: api/Suspects/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var Suspect = await db.Suspects.FindAsync(key);
            if (Suspect == null)
            {
                return NotFound();
            }

            db.Suspects.Remove(Suspect);
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

        private bool SuspectExists(int key)
        {
            return db.Suspects.Any(e => e.Id == key);
        }
    }
}