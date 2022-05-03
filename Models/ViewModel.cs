using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;

namespace mvc.Models
{
     public class ViewModel
    {
        public IEnumerable<Employee> Team { get; set; }
        public IEnumerable<Text> Texts { get; set; }
    }
}