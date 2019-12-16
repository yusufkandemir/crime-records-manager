using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeRecordsManager.Models
{
    [Table("Suspects")]
    public class Suspect: Person
    {
        public string Nationality { get; set; }
        public string FacialAppearance { get; set; }
        public string PhysicalAppearance { get; set; }

        public int? PictureId { get; set; }
        public virtual Attachment Picture { get; set; }
        public virtual ICollection<InvestigationReport> Reports { get; set; }
    }
}