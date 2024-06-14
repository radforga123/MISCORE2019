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

            var patients = new Patient[]
            {
                new Patient{FIO="John Doe"},
                new Patient{FIO="Jane Smith"}
            };

            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

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

            var visits = new Visit[]
            {
                new Visit{time=DateTime.Now, PatientID=patients[0].ID, doctors=new List<Doctor>{doctors[0]}},
                new Visit{time=DateTime.Now.AddDays(1), PatientID=patients[1].ID, doctors=new List<Doctor>{doctors[1]}}
            };

            foreach (Visit v in visits)
            {
                context.Visits.Add(v);
            }
            context.SaveChanges();

            var analyzes = new Analyze[]
            {
                new Analyze{weight=1, diagnos="None", PatientID=patients[0].ID},
                new Analyze{weight=2, diagnos="High", PatientID=patients[1].ID}
            };

            foreach (Analyze a in analyzes)
            {
                context.Analyzes.Add(a);
            }
            context.SaveChanges();
        }

    }
}
