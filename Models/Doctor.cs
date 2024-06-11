using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MISCORE2019.Models
{
    public class Doctor
    {
        public int ID { get; set; }
        [Display(Name = "ФИО")]
        public String FIO { get; set; }

        [Display(Name = "Специализация")]
        public String specialization { get; set; }
    }
}
