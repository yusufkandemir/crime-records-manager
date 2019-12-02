using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrimeRecordsManager.Models
{
    public class Case
    {
        public int Id { get; private set; }
        // WIP

        public virtual PoliceStation StationInCharge { get; set; }
        public virtual InvestigationReport InvestigationReport { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Suspect> Suspects { get; set; }
        public virtual ICollection<Evidence> Evidences { get; set; }
    }
}