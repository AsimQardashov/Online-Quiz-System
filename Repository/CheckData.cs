using ExamingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Repository
{
    public class CheckData
    {
        public bool CheckUserData (int  param)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            var userCkeck = db.Exams.Where(x=>x.UserId==Convert.ToInt32(userid)).FirstOrDefault();
            if (userCkeck!=null)
            {

            }
            return true;
        }
    }
}