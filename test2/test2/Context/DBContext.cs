using Microsoft.EntityFrameworkCore;
using test2.Data;

namespace test2.Context
{
    public class DBContext
    {
        public DbSet<Patient> Patients { get; set; }

    }
}
