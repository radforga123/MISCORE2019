using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISCORE2019.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public String FIO { get; set; }
        public List<Visit> visits { get; set; }
        public List<Analyze> analyzes { get; set; }
    }
    
    
    
}
