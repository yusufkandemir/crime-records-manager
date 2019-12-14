using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CrimeRecordsManager.Models;

namespace CrimeRecordsManager.Controllers
{
    public class PoliceOfficersController : ApiController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/PoliceOfficers
        public IQueryable<PoliceOfficer> GetPoliceOfficers(bool includeStation = false)
        {
            var result = db.PoliceOfficers.AsQueryable();

            if (includeStation)
            {
                result = result.Include(x => x.Station);
            }

            return result;
        }

        // GET: api/PoliceOfficers/5
        [ResponseType(typeof(PoliceOfficer))]
        public async Task<IHttpActionResult> GetPoliceOfficer(int id, bool includeStation = false)
        {
            var query = db.PoliceOfficers.AsQueryable();

            if (includeStation)
            {
                query = query.Include(x => x.Station);
            }

            PoliceOfficer policeOfficer = await query.FirstOrDefaultAsync(x => x.Id == id);
            if (policeOfficer == null)
            {
                return NotFound();
            }

            return Ok(policeOfficer);
        }

        // PUT: api/PoliceOfficers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPoliceOfficer(int id, PoliceOfficer policeOfficer)
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PoliceOfficers
        [ResponseType(typeof(PoliceOfficer))]
        public async Task<IHttpActionResult> PostPoliceOfficer(PoliceOfficer policeOfficer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PoliceOfficers.Add(policeOfficer);
            await db.SaveChangesAsync();

            db.Entry(policeOfficer).Reference(x => x.Station).Load();

            return CreatedAtRoute("DefaultApi", new { id = policeOfficer.Id }, policeOfficer);
        }

        // DELETE: api/PoliceOfficers/5
        [ResponseType(typeof(PoliceOfficer))]
        public async Task<IHttpActionResult> DeletePoliceOfficer(int id)
        {
            PoliceOfficer policeOfficer = await db.PoliceOfficers.FindAsync(id);
            if (policeOfficer == null)
            {
                return NotFound();
            }

            db.PoliceOfficers.Remove(policeOfficer);
            await db.SaveChangesAsync();

            return Ok(policeOfficer);
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
            return db.PoliceOfficers.Count(e => e.Id == id) > 0;
        }
    }
}