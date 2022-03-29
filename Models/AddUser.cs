using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class AddUser
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string UserFullName { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string UserPhoto { get; set; }
        public string UserStatus { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string UserPassword { get; set; }
        public int GenderId { get; set; }
        //[Required(ErrorMessage = "Boş qala bilməz")]
        public string UserMFenn { get; set; }
        public AddUser()
        {
            this.UserDate = DateTime.Now;
        }
        public DateTime UserDate { get; set; }
    }
}