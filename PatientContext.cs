using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MISCORE2019.Models;
namespace MISCORE2019
{
    public class PatientContext:DbContext
    {
        public DbSet<PatientClass> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Analyze> Analyzes { get; set; }
        public PatientContext(DbContextOptions<PatientContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
