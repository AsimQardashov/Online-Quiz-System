using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string LogName { get; set; }
        public Log()
        {
            this.LogDate = DateTime.Now;
        }

        public DateTime LogDate { get; set; }
    }
}