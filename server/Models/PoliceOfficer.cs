using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeRecordsManager.Models
{
    [Table("PoliceOfficers")]
    public class PoliceOfficer: Person
    {
        public string Rank { get; set; }

        public int? StationId { get; set; }
        public virtual PoliceStation Station { get; set; }
    }
}