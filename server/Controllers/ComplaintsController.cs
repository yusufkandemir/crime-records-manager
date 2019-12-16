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
    public class ComplaintsController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/Complaints
        [EnableQuery(MaxTop = 50)]
        public IQueryable<Complaint> Get()
        {
            return db.Complaints;
        }

        // GET: api/Complaints/5
        [EnableQuery]
        public SingleResult<Complaint> Get([FromODataUri] int key)
        {
            IQueryable<Complaint> result = db.Complaints.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }

        // POST: api/Complaints
        public async Task<IHttpActionResult> Post(Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Complaints.Add(complaint);
            await db.SaveChangesAsync();

            return Created(complaint);
        }

        // PUT: api/Complaints/5
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != complaint.Id)
            {
                return BadRequest();
            }

            db.Entry(complaint).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(complaint);
        }

        // PATCH: api/Complaints/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Complaint> complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Complaints.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            complaint.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(key))
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

        // DELETE: api/Complaints/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var Complaint = await db.Complaints.FindAsync(key);
            if (Complaint == null)
            {
                return NotFound();
            }

            db.Complaints.Remove(Complaint);
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

        private bool ComplaintExists(int key)
        {
            return db.Complaints.Any(e => e.Id == key);
        }
    }
}