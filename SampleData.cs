using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISCORE2019.Models;

namespace MISCORE2019
{
    public class SampleData
    {
        public static void Initialize(PatientContext context)
        {
            if (!context.Patients.Any())
            {
                context.Doctors.AddRange(
                    

                );
                context.SaveChanges();
            }
        }
    }
}
