using System;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
     public class SystemUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}