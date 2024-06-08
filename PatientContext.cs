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
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Analyze> Analyzes { get; set; }
        public PatientContext(DbContextOptions<PatientContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Patient)
                .WithMany(p => p.visits)
                .HasForeignKey(v => v.PatientID);

            modelBuilder.Entity<Analyze>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.analyzes)
                .HasForeignKey(a => a.PatientID);
        }

    }
}
