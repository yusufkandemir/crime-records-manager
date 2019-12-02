using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrimeRecordsManager.Models
{
    public class Suspect
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string PicturePath { get; set; }
        public string FacialAppearance { get; set; }
        public string PhysicalAppearance { get; set; }

        public virtual ICollection<InvestigationReport> Reports { get; set; }
    }
}