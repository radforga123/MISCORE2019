﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MISCORE2019.Models
{
    public class Visit
    {
        public int ID { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime time { get; set; }
        public List<Doctor> doctors { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}
