using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class Exam_m_
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public int CQId { get; set; }

        public Exam_m_()
        {
            this.ExamDate = DateTime.Now;
        }
        public DateTime ExamDate { get; set; }
    }
}