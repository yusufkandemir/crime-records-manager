﻿using System.Collections.Generic;

namespace CrimeRecordsManager.Models
{
    public class PoliceStation
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual PoliceOfficer HeadOfficer { get; set; }
        public virtual ICollection<PoliceOfficer> Officers { get; set; }
    }
}