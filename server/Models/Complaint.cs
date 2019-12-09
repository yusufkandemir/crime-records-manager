﻿using System;

namespace CrimeRecordsManager.Models
{
    public class Complaint
    {
        public int Id { get; private set; }
        public string Details { get; set; }
        public string Place { get; set; }
        public DateTime FiledAt { get; set; }
        
        public Person Complainant { get; set; }
        public virtual PoliceStation PoliceStation { get; set; }
    }
}