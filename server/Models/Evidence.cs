using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrimeRecordsManager.Models
{
    public class Evidence
    {
        public int Id { get; private set; }
        public string Description { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentPath { get; set; }

        public virtual Case Case { get; set; }
    }
}