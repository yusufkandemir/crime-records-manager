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
    public class ComplainantsController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/Complainants
        [EnableQuery(MaxTop = 50)]
        public IQueryable<Complainant> Get()
        {
            return db.Complainants;
        }

        // GET: api/Complainants/5
        [EnableQuery]
        public SingleResult<Complainant> Get([FromODataUri] int key)
        {
            IQueryable<Complainant> result = db.Complainants.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }

        // POST: api/Complainants
        public async Task<IHttpActionResult> Post(Complainant complainant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Complainants.Add(complainant);
            await db.SaveChangesAsync();

            return Created(complainant);
        }

        // PUT: api/Complainants/5
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Complainant complainant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != complainant.Id)
            {
                return BadRequest();
            }

            db.Entry(complainant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplainantExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(complainant);
        }

        // PATCH: api/Complainants/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Complainant> complainant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Complainants.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            complainant.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplainantExists(key))
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

        // DELETE: api/Complainants/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var Complainant = await db.Complainants.FindAsync(key);
            if (Complainant == null)
            {
                return NotFound();
            }

            db.Complainants.Remove(Complainant);
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

        private bool ComplainantExists(int key)
        {
            return db.Complainants.Any(e => e.Id == key);
        }
    }
}