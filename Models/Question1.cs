using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Question1
    {
        public int QId { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string QText { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public string VariantText { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]
        public int CQId { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz")]

        public int VariantId { get; set; }

        public Question1()
        {
            this.QuestionDate = DateTime.Now;
        }
        public DateTime QuestionDate { get; set; }

    }
}
