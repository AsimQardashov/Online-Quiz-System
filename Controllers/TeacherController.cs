using ExamingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamingSystem.Controllers
{
    [Authorize(Roles="M")]
    public class TeacherController : Controller
    {
        // GET: Teacher
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult TeacherCheck()
        {
            student_answer student_Answer = new student_answer();
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            //var Quest = Sql.ExecuteOne($@"Select * from student_answer");
            //int countQ = Quest.Rows.Count;
            //var examId = Quest.Rows[2]["ExamId"];
            //var cq = Sql.ExecuteOne($@"Select CQId from Exam where ExamId='" + Convert.ToInt32(examId) + "' ");
            //var teacher = Sql.ExecuteOne($@"Select * from Users Where UserMFenn='1' and UserStatus='active' and UserRole='M' ");
            //int countT = teacher.Rows.Count;
            //int top = countQ / countT;
            var  a = Sql.ExecuteOne($@"Select * from student_answer");
            ViewBag.a = a;
            if (ViewBag.a.Rows.Count==0)
            {
                ViewBag.empty = "Qiymətləndiriləcək heçnə yoxdur..";
            }
            
            //for (int i = 0; i < top; i++)
            //{
            //    student_Answer = db.student_answers.FirstOrDefault();
            //    student_Answer.GiveT = 1;
            //    db.SubmitChanges();
            //    ViewBag.a = a;
            //}
            return View();
        }
        [HttpPost]
        public ActionResult TeacherCheck(int[] OPId, int[] ExamId, string[] TeacherPoint, Log log1)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            if (TeacherPoint != null)
            {
                for (int i = 0; i < TeacherPoint.Length; i++)
                {
                    if (TeacherPoint[i] != "")
                    {
                        OpenQuestionsAnswer openQuestionsAnswer = db.OpenQuestionsAnswers.Single(x => x.OPId == OPId[i] && x.ExamId == ExamId[i]);
                        openQuestionsAnswer.AnswerStatus = "1";
                        openQuestionsAnswer.TeacherPoint = Convert.ToInt32(TeacherPoint[i]);
                        openQuestionsAnswer.TeacherId = Convert.ToInt32(userid);
                        db.SubmitChanges();
                    }
                }
                Loglar log = new Loglar();
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Teacher Checked";
                log.LogDate = log1.LogDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            return RedirectToAction(nameof(TeacherCheck));
        }
        public ActionResult TeacherProfile()
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.Teacher = Sql.ExecuteOne($@"Select UserName, UserFullName, UserEmail, UserMFenn, UserDate, UserPhoto from Users Where UserId='"+Convert.ToInt32(userid)+"' ;");
            return View();
        }
        [HttpPost]
        public ActionResult TeacherProfile(User user)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User user1 = new User();
            User useredit = db.Users.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
            useredit.UserFullName = user.UserFullName;
            useredit.UserName = user.UserName;
            useredit.UserEmail = user.UserEmail;
            db.SubmitChanges();
            return RedirectToAction(nameof(TeacherProfile));
        }
    }
}