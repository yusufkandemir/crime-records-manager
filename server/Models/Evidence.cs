using System;

namespace CrimeRecordsManager.Models
{
    public class Evidence
    {
        public int Id { get; private set; }
        public string Description { get; set; }
        public DateTime GatheredAt { get; set; }

        public virtual Case Case { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}