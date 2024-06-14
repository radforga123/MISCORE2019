using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MISCORE2019.Models
{
    public class Patient
    {
        public int ID { get; set; }
        [Display(Name = "ФИО")]
        public String FIO { get; set; }
        [Display(Name="Посещения")]
        public List<Visit> visits { get; set; }
        [Display(Name = "Анализы")]
        public List<Analyze> analyzes { get; set; }
    }
    
    
    
}
