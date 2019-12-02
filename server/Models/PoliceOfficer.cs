using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrimeRecordsManager.Models
{
    public class PoliceOfficer
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }

        public virtual PoliceStation Station { get; set; }
    }
}