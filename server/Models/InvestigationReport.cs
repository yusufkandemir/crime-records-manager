using System;

namespace CrimeRecordsManager.Models
{
    public class InvestigationReport
    {
        public int Id { get; set; }
        public string Act { get; set; }
        public string Address { get; set; }
        public DateTime OccuredAt { get; set; }
        public DateTime WrittenAt { get; set; }

        public virtual PoliceOfficer Reporter { get; set; }
    }
}