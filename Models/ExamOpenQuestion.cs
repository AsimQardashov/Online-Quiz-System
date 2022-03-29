using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamingSystem.Models
{
    public class ExamOpenQuestion
    {
        public List<OpenQuestion> openQuestions { get; set; }
        public List<OpenQuestionsAnswer> openQuestionsAnswers { get; set; }

    }
}