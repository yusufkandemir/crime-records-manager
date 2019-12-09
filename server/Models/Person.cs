using System;

namespace CrimeRecordsManager.Models
{
	public abstract class Person
	{
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}