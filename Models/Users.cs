using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    public  class Users
    {
        public int UserId { get; internal set; }

        [Required(ErrorMessage = "Istifadeci adi bos ola bilmez")]
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public int GenderId { get; set; }

        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage="Dogru formatda Email adresi daxil edin")]

        public string UserEmail { get; set; }
        public string UserPhoto { get; set; }
        public Users()
        {
            this.UserDate = DateTime.Now;
        }
        public DateTime UserDate { get; set; }
    }
}