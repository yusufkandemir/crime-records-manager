using System.Collections.Generic;

namespace CrimeRecordsManager.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual PoliceStation StationInCharge { get; set; }
        public virtual InvestigationReport InvestigationReport { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Suspect> Suspects { get; set; }
        public virtual ICollection<Evidence> Evidences { get; set; }
    }
}