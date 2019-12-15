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
    public class PoliceStationsController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/PoliceStations
        [EnableQuery(MaxTop = 50)]
        public IQueryable<PoliceStation> Get()
        {
            return db.PoliceStations;
        }

        // GET: api/PoliceStations/5
        [EnableQuery]
        public SingleResult<PoliceStation> Get([FromODataUri] int key)
        {
            IQueryable<PoliceStation> result = db.PoliceStations.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }

        // POST: api/PoliceStations
        public async Task<IHttpActionResult> Post(PoliceStation policeStation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PoliceStations.Add(policeStation);
            await db.SaveChangesAsync();

            return Created(policeStation);
        }

        // PUT: api/PoliceStations/5
        public async Task<IHttpActionResult> Put([FromODataUri] int key, PoliceStation policeStation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != policeStation.Id)
            {
                return BadRequest();
            }

            db.Entry(policeStation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliceStationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(policeStation);
        }

        // PATCH: api/PoliceStations/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<PoliceStation> policeStation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.PoliceStations.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            policeStation.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliceStationExists(key))
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

        // DELETE: api/PoliceStations/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var policeStation = await db.PoliceStations.FindAsync(key);
            if (policeStation == null)
            {
                return NotFound();
            }

            db.PoliceStations.Remove(policeStation);
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

        private bool PoliceStationExists(int key)
        {
            return db.PoliceStations.Any(e => e.Id == key);
        }
    }
}