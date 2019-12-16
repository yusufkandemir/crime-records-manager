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
    public class InvestigationReportsController : ODataController
    {
        private CrimeContext db = new CrimeContext();

        // GET: api/InvestigationReports
        [EnableQuery(MaxTop = 50)]
        public IQueryable<InvestigationReport> Get()
        {
            return db.InvestigationReport;
        }

        // GET: api/InvestigationReports/5
        [EnableQuery]
        public SingleResult<InvestigationReport> Get([FromODataUri] int key)
        {
            IQueryable<InvestigationReport> result = db.InvestigationReport.Where(p => p.Id == key);

            return SingleResult.Create(result);
        }

        // POST: api/PoliceOfficers
        public async Task<IHttpActionResult> Post(InvestigationReport investigationReport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InvestigationReport.Add(investigationReport);
            await db.SaveChangesAsync();

            return Created(investigationReport);
        }

        // PUT: api/PoliceOfficers/5
        public async Task<IHttpActionResult> Put([FromODataUri] int key, InvestigationReport investigationReport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != investigationReport.Id)
            {
                return BadRequest();
            }

            db.Entry(investigationReport).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvestigationReportExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(investigationReport);
        }

        // PATCH: api/investigationReports/5
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<InvestigationReport> investigationReport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.InvestigationReport.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            investigationReport.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvestigationReportExists(key))
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

        // DELETE: api/InvestigationReports/5
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var investigationReport = await db.InvestigationReport.FindAsync(key);
            if (investigationReport == null)
            {
                return NotFound();
            }

            db.InvestigationReport.Remove(investigationReport);
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

        private bool InvestigationReportExists(int key)
        {
            return db.InvestigationReport.Any(e => e.Id == key);
        }
    }
}