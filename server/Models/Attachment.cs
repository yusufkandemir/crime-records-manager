using System;

namespace CrimeRecordsManager.Models
{
    public class Attachment
    {
        public int Id { get; private set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}