﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MISCORE2019.Models
{
    public class Analyze
    {
        public int ID { get; set; }

        [Display(Name = "Вес")]
        public int weight { get; set; }

        [Display(Name = "Диагноз")]
        public String diagnos { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}
