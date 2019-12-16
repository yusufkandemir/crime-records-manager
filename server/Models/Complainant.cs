using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeRecordsManager.Models
{
    [Table("Complainants")]
    public class Complainant : Person
    {
        public string PhoneNumber { get; set; }

        public virtual ICollection<Complaint> Complaints { get; set; }
    }
}