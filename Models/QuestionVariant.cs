using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class QuestionVariant
    {
        public QuestionsTest QuestionsTest { get; set; }
        public QuestionsOpen QuestionsOpen  { get; set; }

        public List<Variant> Variants { get; set; }
    }
}