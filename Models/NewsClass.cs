using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class NewsClass
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsText { get; set; }
        public string NewsPhoto { get; set; }
        public DateTime NewsDate { get; set; }
        
        public NewsClass()
        {
            this.NewsDate = DateTime.Now;
        }
    }
}