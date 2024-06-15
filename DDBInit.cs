using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISCORE2019.Models;
namespace MISCORE2019
{
    public class DDBInit
    {
        public static void Initialize(PatientContext context)
        {
            context.Database.EnsureCreated();

            if (context.Patients.Any())
            {
                return;  
            }

            
            var doctors = new Doctor[]
            {
                new Doctor{FIO="Dr. Brown", specialization="Cardiology"},
                new Doctor{FIO="Dr. White", specialization="Neurology"}
            };

            foreach (Doctor d in doctors)
            {
                context.Doctors.Add(d);
            }
            context.SaveChanges();

            
            context.SaveChanges();
        }

    }
}
