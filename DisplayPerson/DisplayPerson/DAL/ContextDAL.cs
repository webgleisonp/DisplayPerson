using System.Data.Entity;
using DisplayPerson.Models;

namespace DisplayPerson.DAL
{
    public class ContextDAL : DbContext
    {
        public ContextDAL()
            : base("DisplayPerson")
        {

        }

        public DbSet<Person> Persons { get; set; }
    }
}