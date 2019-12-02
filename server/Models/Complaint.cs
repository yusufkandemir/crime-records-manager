using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrimeRecordsManager.Models
{
    public class Complaint
    {
        public int Id { get; private set; }
        [MinLength(64), MaxLength(2048)]
        public string Details { get; set; }
        public string Place { get; set; }
        public DateTime FiledAt { get; set; }
        public string ComplainantFullName { get; set; }

        public virtual PoliceStation PoliceStation { get; set; }
    }
}