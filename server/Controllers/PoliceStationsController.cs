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
    public class PoliceStationsController : ApiController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/PoliceStations
        public IQueryable<PoliceStation> GetPoliceStations(bool includeOfficers = false)
        {
            var result = db.PoliceStations.AsQueryable();

            if (includeOfficers)
            {
                result = result.Include(x => x.Officers);
            }

            return result;
        }

        // GET: api/PoliceStations/5
        [ResponseType(typeof(PoliceStation))]
        public async Task<IHttpActionResult> GetPoliceStation(int id, bool includeOfficers = false)
        {
            var query = db.PoliceStations.AsQueryable();

            query = query.Include(x => x.HeadOfficer);

            if (includeOfficers)
            {
                query = query.Include(x => x.Officers);
            }

            PoliceStation policeStation = await query.FirstOrDefaultAsync(x => x.Id == id);
            if (policeStation == null)
            {
                return NotFound();
            }

            return Ok(policeStation);
        }

        // PUT: api/PoliceStations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPoliceStation(int id, PoliceStation policeStation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != policeStation.Id)
            {
                return BadRequest();
            }

            db.Entry(policeStation).State = EntityState.Modified;

            var headOfficer = await db.PoliceOfficers.FindAsync(policeStation.HeadOfficer.Id);
            if (headOfficer != null)
            {
                headOfficer.Station = policeStation;
                policeStation.HeadOfficer = headOfficer;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliceStationExists(id))
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

        // POST: api/PoliceStations
        [ResponseType(typeof(PoliceStation))]
        public async Task<IHttpActionResult> PostPoliceStation(PoliceStation policeStation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var headOfficer = await db.PoliceOfficers.FindAsync(policeStation.HeadOfficer.Id);
            if (headOfficer != null)
            {
                headOfficer.Station = policeStation;
                policeStation.HeadOfficer = headOfficer;
            }

            db.PoliceStations.Add(policeStation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = policeStation.Id }, policeStation);
        }

        // DELETE: api/PoliceStations/5
        [ResponseType(typeof(PoliceStation))]
        public async Task<IHttpActionResult> DeletePoliceStation(int id, bool includeOfficers = false)
        {
            PoliceStation policeStation = await db.PoliceStations.FindAsync(id);
            if (policeStation == null)
            {
                return NotFound();
            }

            if (includeOfficers)
            {
                db.PoliceOfficers.RemoveRange(policeStation.Officers);
            }

            db.PoliceStations.Remove(policeStation);
            await db.SaveChangesAsync();

            return Ok(policeStation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PoliceStationExists(int id)
        {
            return db.PoliceStations.Count(e => e.Id == id) > 0;
        }
    }
}