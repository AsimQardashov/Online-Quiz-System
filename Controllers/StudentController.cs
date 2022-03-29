using ExamingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamingSystem.Controllers
{
    [Authorize(Roles="T")]
    public class StudentController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Student
        public ActionResult CreateExam()
        {
            ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
            return View();
        }
        [HttpPost]
        public ActionResult CreateExam(Exam_m_ exam)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name; ;
            Exam ex = new Exam();
            if (ModelState.IsValid)
            {
                ex.CQId = exam.CQId;
                ex.ExamDate = exam.ExamDate;
                ex.UserId = Convert.ToInt32(userid);
                ex.UserIPd=Request.UserHostAddress;
                db.Exams.InsertOnSubmit(ex);
                db.SubmitChanges();
                ViewBag.ExamId = ex.ExamId;
            }
            Variant v = new Variant();
            int fenn = exam.CQId;
            DataTable Tests = Sql.ExecuteOne($@"select top 5 QId,QText from QuestionsTest where QStatus = 'active' and CQId = '" +
                fenn + "' order by newid()");
            DataTable OpenQ = Sql.ExecuteOne($@"select top 5 QOId,QOText from QuestionsOpen where QOStatus = 'active' and CQId = '" + 
                fenn + "' and AdminConturol=1 order by newid()");
            DataRow[] dra = new DataRow[Tests.Rows.Count];
            Tests.Rows.CopyTo(dra, 0);
            List<QuestionVariant> list = new List<QuestionVariant>();
            for (int i = 0; i < dra.Length; i++)
            {
                ExamQuestion eq = new ExamQuestion();
                int q = Convert.ToInt32(Tests.Rows[i]["QId"]);
                eq.QId = q;
                eq.UserId = Convert.ToInt32(userid);
                eq.ExamId = ex.ExamId;
                QuestionVariant questionVariant = new QuestionVariant();
                QuestionsTest question = new QuestionsTest();
                QuestionsOpen questionsOpen = new QuestionsOpen();
                question.QId = dra[i].Field<int>("QId");
                question.QText = dra[i].Field<string>("QText");
                questionVariant.QuestionsTest = question;
                questionsOpen.QOText = OpenQ.Rows[i]["QOText"].ToString();
                questionsOpen.QOId = Convert.ToInt32(OpenQ.Rows[i]["QOId"]);
                questionVariant.QuestionsOpen = questionsOpen;
                DataTable Variants = Sql.ExecuteOne($@"select  QId,VariantText,VariantId from Variants where QId = '" + 
                                                        questionVariant.QuestionsTest.QId + "'");
                DataRow[] dra1 = new DataRow[Variants.Rows.Count];
                Variants.Rows.CopyTo(dra1, 0);
                List<Variant> variants = new List<Variant>();
                for (int j = 0; j < dra1.Length; j++)
                {
                    Variant variant = new Variant();
                    variant.QId = dra1[j].Field<int>("QId");
                    variant.VariantId = dra1[j].Field<int>("VariantId");
                    variant.VariantText = dra1[j].Field<string>("VariantText");
                    variants.Add(variant);
                }
                eq.QId = question.QId;
                eq.QOId = questionsOpen.QOId;
                db.ExamQuestions.InsertOnSubmit(eq);
                db.SubmitChanges();
                questionVariant.Variants = variants;
                list.Add(questionVariant);
            }
            Loglar log = new Loglar();
            log.UserId = Convert.ToInt32(userid);
            log.LogName = "Start Exam";
            log.LogDate = exam.ExamDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return View("StartExam", list);
        }
        [HttpPost]
        public ActionResult StartExam(int[] QId, int[] VariantId, int ExamId, int[] QOId, string[] SutudentAnswer, Log log1)
        {
            ExamQuestion examQuestion = new ExamQuestion();
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            if (VariantId != null)
            {
                for (int i = 0; i < VariantId.Length; i++)
                {
                    int qid = Convert.ToInt32(db.Variants.Single(x => x.VariantId == VariantId[i]).QId);
                    ExamQuestion examquestion = db.ExamQuestions.Single(x => x.QId == qid && x.ExamId == ExamId);

                    examquestion.SelectedAnswer = VariantId[i].ToString();
                    db.SubmitChanges();
                }
            }
            for (int i = 0; i < QOId.Length; i++)
            {
                OpenQuestionsAnswer openQuestionsAnswer = new OpenQuestionsAnswer();
                openQuestionsAnswer.OPId = QOId[i];
                openQuestionsAnswer.StudentId = Convert.ToInt32(userid);
                openQuestionsAnswer.ExamId = ExamId;
                openQuestionsAnswer.AnswerStatus = "0";
                openQuestionsAnswer.SutudentAnswer = SutudentAnswer[i];
                openQuestionsAnswer.GiveT = 0;
                db.SubmitChanges();
                db.OpenQuestionsAnswers.InsertOnSubmit(openQuestionsAnswer);
            }
            var scor = 0;
            ViewBag.Scor = Sql.ExecuteOne($@"Select * from ExamQuestions where ExamId ='" + ExamId + "' and TrueOrFalse='1'");
            for (int i = 0; i < ViewBag.Scor.Rows.Count; i++)
            {
                scor+=10;
            }
            TotalScore totalScore = new TotalScore();
            totalScore.UserId = Convert.ToInt32(userid);
            totalScore.Score = scor.ToString();
            totalScore.ExamId = ExamId;
            db.TotalScores.InsertOnSubmit(totalScore);
            Loglar log = new Loglar();
            log.UserId = Convert.ToInt32(userid);
            log.LogName = "Finish Exam";
            log.LogDate = log1.LogDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return RedirectToAction("StudentProfle", new { ExamId = totalScore.ExamId });
        }
        public ActionResult YourScore(int? ExamId)
        {
            if (ExamId != 0)
            {
                ViewBag.SCOR = db.TotalScores.Where(x => x.ExamId == ExamId).FirstOrDefault().Score;
            }
            return View();
        }
        public ActionResult StudentProfle()
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.User = Sql.ExecuteOne($@"Select * from Users where UserId='"+ Convert.ToInt32(userid) +"' ");
            ViewBag.stdscore = Sql.ExecuteOne($@"select Users.UserFullName, Users.UserName , TotalScore.UserId,TotalScore.ExamId , 
                        TotalScore.Score,TotalScore.ScorOpen,TotalScore.TotalScore,TotalScore.Performance, 
                        CategoryQuestions.CQName, Exam.ExamDate from TotalScore
                        join Users on Users.UserID=TotalScore.UserId
                        join Exam on Exam.ExamId=TotalScore.ExamId
                        join CategoryQuestions on CategoryQuestions.CQId=Exam.CQId where Users.UserId='" + Convert.ToInt32(userid) + "'");
            ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
            return View();
        }
        [HttpPost]
        public ActionResult StudentProfle(User users)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.edit = Sql.ExecuteOne($@"select * from Users where Users.UserId='" + Convert.ToInt32(userid) + "'");
            User user = new User();
            User useredit = db.Users.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
            useredit.UserFullName = users.UserFullName;
            useredit.UserName = users.UserName;
            useredit.UserEmail = users.UserEmail;
            db.SubmitChanges();
            return RedirectToAction(nameof(StudentProfle));
        }
    }
}




