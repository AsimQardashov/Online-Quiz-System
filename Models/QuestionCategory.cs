using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class QuestionCategory
    {
        public int CQId { get; set; }
        [Required(ErrorMessage ="bos olmaz")]
        public string CQName{ get; set; }
        
        public QuestionCategory()
        {
            this.CategoryDate = DateTime.Now;
        }
        public DateTime CategoryDate { get; set; }
    }
}