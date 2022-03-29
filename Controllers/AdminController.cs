using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Security;
using ExamingSystem.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace ExamingSystem.Controllers
{
    public class AdminController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Admin
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.User = Sql.ExecuteOne($@"select * from Gender");
            return View();
        }
        [HttpPost]
        public ActionResult Login(User k)
        {
            string MD5Password = Password.MD5Creating(k.UserPassword);
            var user = db.Users.FirstOrDefault(x => x.UserName == k.UserName && x.UserPassword == MD5Password);
            
            if (user != null)
            {
                FormsAuthenticationTicket authenticationTicket = new
                     FormsAuthenticationTicket
                     (
                     1,
                     user.UserId.ToString(),
                     DateTime.Now,
                     DateTime.Now.AddMinutes(90),
                     false,
                     k.UserName + k.UserFullName + " admin",
                     FormsAuthentication.FormsCookiePath
                     );
                string hash = FormsAuthentication.Encrypt(authenticationTicket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                HttpContext.Response.Cookies.Add(cookie);
                Loglar log = new Loglar();
                log.LogName = "Login";
                log.UserId = user.UserId;
                log.LogDate = user.UserDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
                return RedirectToAction("Home", "Admin");
            }
            TempData["error"] = "Istifadeci adi veya sifre sehvdir";
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View(nameof(Login));
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(User k)
        {
            var model = db.Users.Where(x => x.UserEmail == k.UserEmail).FirstOrDefault();
            if (model != null)
            {
                string from, pass, messageBody;
                Guid random = Guid.NewGuid();
                model.UserPassword = Password.MD5Creating(random.ToString());
                db.SubmitChanges();
                MailMessage message = new MailMessage();
                string to = (model.UserEmail).ToString();
                from = "mrsnobody27@gmail.com";
                pass = "hmgtohldjydoeetu";
                messageBody = "Sizin yeni şifrəniz " + random;
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "Şifrə yeniləmə istəyi";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, pass);
                smtp.Send(message);
            }
            ViewBag.hata = "Bu mail adresi tapılmadı";
            return RedirectToAction(nameof(ResetPassword));
        }
        public ActionResult Register()
        {
            ViewBag.User = Sql.ExecuteOne($@"select * from Gender");
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users users)
        {
            if (ModelState.IsValid)
            {
                var newuser = db.Users.Where(x => x.UserName == users.UserName || x.UserEmail == users.UserEmail).FirstOrDefault();
                if (newuser == null)
                {
                    users.UserPassword = Password.MD5Creating(users.UserPassword);
                    users.UserPassword = users.UserPassword;

                    User k = new User();
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + Request.Files[0].FileName);
                        string _path = Path.Combine(("~/Image"), _FileName);
                        Request.Files[0].SaveAs(HttpContext.Server.MapPath(_path));
                        k.UserPhoto = _FileName;
                    }
                    k.UserName = users.UserName;
                    k.UserFullName = users.UserFullName;
                    k.UserPassword = users.UserPassword;
                    k.GenderId = users.GenderId;
                    k.UserRole = users.UserRole;
                    k.UserDate = users.UserDate;
                    k.UserEmail = users.UserEmail;
                    k.UserStatus = "active";
                    db.Users.InsertOnSubmit(k);
                    Loglar log = new Loglar();
                    db.SubmitChanges();
                    log.UserId = k.UserId;
                    log.LogName = "Register";
                    log.LogDate = users.UserDate;
                    db.Loglars.InsertOnSubmit(log);
                    db.SubmitChanges();
                    ViewBag.User = Sql.ExecuteOne($@"select * from Gender");
                    ViewBag.already = "Uğurla qeydiyyatdan keçdiniz";
                    return View();
                }
                else
                {
                    ViewBag.User = Sql.ExecuteOne($@"select * from Gender");
                    ViewBag.already = "Bu istifadəçi adı artiq istifadə olunub!";
                    return View();
                }

            }
            return RedirectToAction(nameof(Register));
        }
        [Authorize(Roles = "A")]
        public ActionResult AddUser()
        {
            ViewBag.Muellim = Sql.ExecuteOne($@"Select * from Users Join CategoryQuestions on CQId=UserMFenn Join Gender on Users.GenderId=Gender.GenderId  where UserRole='M' and UserStatus='active';");
            ViewBag.Fenn = Sql.ExecuteOne($@"select * from CategoryQuestions");
            ViewBag.Gender = Sql.ExecuteOne($@"Select * from Gender");
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(AddUser newuser)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User us = new User();
            if (ModelState.IsValid)
            {
                newuser.UserPassword = Password.MD5Creating(newuser.UserPassword);
                newuser.UserPassword = newuser.UserPassword;
                Loglar log = new Loglar();
                us.UserName = newuser.UserName;
                us.UserFullName = newuser.UserFullName;
                us.UserEmail = newuser.UserEmail;
                us.UserMFenn = newuser.UserMFenn;
                us.UserDate = newuser.UserDate;
                us.GenderId = newuser.GenderId;
                us.UserRole = "M";
                us.UserStatus = "active";
                us.UserPassword = newuser.UserPassword;
                db.Users.InsertOnSubmit(us);
                log.UserId =Convert.ToInt32(userid);
                log.LogName = "Teacher Added";
                log.LogDate = newuser.UserDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            return RedirectToAction(nameof(AddUser));
        }
        [Authorize(Roles = "A")]
        public ActionResult AddUserT()
        {
            ViewBag.Telebe = Sql.ExecuteOne($@"Select * from Users join Gender on Users.GenderId=Gender.GenderId where UserRole='T' and UserStatus='active';");
            ViewBag.Gender = Sql.ExecuteOne($@"Select * from Gender");
            return View();
        }
        [HttpPost]
        public ActionResult AddUserT(AddUser newuser)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            if (ModelState.IsValid)
            {
                newuser.UserPassword = Password.MD5Creating(newuser.UserPassword);
                newuser.UserPassword = newuser.UserPassword;
                User us = new User();
                Loglar log = new Loglar();
                us.UserName = newuser.UserName;
                us.UserFullName = newuser.UserFullName;
                us.UserEmail = newuser.UserEmail;
                us.UserDate = newuser.UserDate;
                us.GenderId = newuser.GenderId;
                us.UserRole = "T";
                us.UserStatus = "active";
                us.UserPassword = newuser.UserPassword;
                db.Users.InsertOnSubmit(us);
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Student added";
                log.LogDate = newuser.UserDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            return RedirectToAction(nameof(AddUserT));
        }
        [Authorize(Roles = "A")]
        public ActionResult Edit(long id)
        {
            User user = new User();
            user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            ViewBag.Gender = Sql.ExecuteOne($@"Select * from Gender");
            ViewBag.Fenn = Sql.ExecuteOne($@"Select * from CategoryQuestions");
            return View("Edit", user);
        }
        [HttpPost]
        public ActionResult Edit(User users, Log log1)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User useredit = db.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
            useredit.UserFullName = users.UserFullName;
            useredit.GenderId = users.GenderId;
            useredit.UserName = users.UserName;
            useredit.UserEmail = users.UserEmail;
            useredit.UserMFenn = users.UserMFenn;
            Loglar log = new Loglar();
            log.UserId = Convert.ToInt32(userid);
            log.LogName = "Teacher Edit";
            log.LogDate = log1.LogDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUser));
        }
        public ActionResult EditT(long id)
        {
            User user = new User();
            user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            ViewBag.Gender = Sql.ExecuteOne($@"Select * from Gender");
            return View("EditT", user);
        }
        [HttpPost]
        public ActionResult EditT(User users, Log log1)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User useredit = db.Users.Where(x => x.UserId == users.UserId).FirstOrDefault();
            useredit.UserFullName = users.UserFullName;
            useredit.UserName = users.UserName;
            useredit.GenderId = users.GenderId;
            useredit.UserEmail = users.UserEmail;
            useredit.UserMFenn = users.UserMFenn;
            Loglar log = new Loglar();
            log.LogName = "Student Edit";
            log.LogDate = log1.LogDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUserT));
        }
        [Authorize(Roles = "A")]
        public ActionResult Delete(int id, Log log1)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User user = new User();
            user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            user.UserStatus = "passive";
            Loglar log = new Loglar();
            log.LogName = "Teacher Deleted";
            log.LogDate = log1.LogDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUser));
        }
        [HttpPost]
        public ActionResult Delete(User user)
        {
            User userdelete = db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            userdelete.UserStatus = "passive";
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUser));
        }
        [Authorize(Roles = "A")]
        public ActionResult DeleteT(int id, Log log1)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User user = new User();
            user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            user.UserStatus = "passive";
            Loglar log = new Loglar();
            log.LogName = "Student Deleted";
            log.LogDate = log1.LogDate;
            db.Loglars.InsertOnSubmit(log);
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUserT));
        }
        [HttpPost]
        public ActionResult DeleteT(User user)
        {
            User userdelete = db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            userdelete.UserStatus = "passive";
            db.SubmitChanges();
            return RedirectToAction(nameof(AddUserT));
        }
        public ActionResult AddQuestion()
        {
            ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
            return View();
        }
        [HttpPost]
        public ActionResult AddQuestion(Question1 question, string[] VariantText, int VariantId, Log log1)
        {
            if (ModelState.IsValid && VariantText[0]!="")
            {
                string userid = System.Web.HttpContext.Current.User.Identity.Name;
                QuestionsTest q = new QuestionsTest();
                q.QStatus = "active";
                q.QText = question.QText;
                q.CQId = question.CQId;
                q.QuestionDate = question.QuestionDate;
                q.AdminConturol = 0;
                db.QuestionsTests.InsertOnSubmit(q);
                Loglar log = new Loglar();
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Test Question Added";
                log.LogDate = log1.LogDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
                for (int i = 0; i < 5; i++)
                {
                    Variant v = new Variant();
                    v.VariantText = VariantText[i];
                    v.QId = q.QId;
                    v.OptId = i + 1;
                    if (v.OptId == VariantId)
                    {
                        v.Correct = "1";
                    }
                    else
                    {
                        v.Correct = "0";
                    }
                    db.Variants.InsertOnSubmit(v);
                    db.SubmitChanges();
                }
                ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
                return RedirectToAction(nameof(AddQuestion));

            }
            ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
            ViewBag.Error = "Bütün xanaları doldurun";
            return View(nameof(AddQuestion));
        }
        public ActionResult AddQuestionOpen()
        {
            ViewBag.categories = Sql.ExecuteOne($@"Select * from CategoryQuestions;");
            return View();
        }
        [HttpPost]
        public ActionResult AddQuestionOpen(OpenQuestion questionsOpen, Log log1)
        {
            if (ModelState.IsValid)
            {
                string userid = System.Web.HttpContext.Current.User.Identity.Name;
                QuestionsOpen qo = new QuestionsOpen();
                qo.QOText = questionsOpen.QOText;
                qo.CQId = questionsOpen.CQId;
                qo.QOStatus = "active";
                qo.AdminConturol = 0;
                db.QuestionsOpens.InsertOnSubmit(qo);
                Loglar log = new Loglar();
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Open Question Added";
                log.LogDate = log1.LogDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
                return RedirectToAction(nameof(AddQuestionOpen));
            }
            return View(nameof(AddQuestionOpen));
        }
        [Authorize(Roles = "A")]
        public ActionResult AddQuestionCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddQuestionCategory(QuestionCategory categoryQuestion)
        {
            if (ModelState.IsValid)
            {
                string userid = System.Web.HttpContext.Current.User.Identity.Name;
                CategoryQuestion cq = new CategoryQuestion();
                cq.CQName = categoryQuestion.CQName;
                cq.CategoryDate = categoryQuestion.CategoryDate;
                cq.CStatus = "active";
                db.CategoryQuestions.InsertOnSubmit(cq);
                Loglar log = new Loglar();
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Category Added";
                log.LogDate = categoryQuestion.CategoryDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
                return View(categoryQuestion);
            }
            return View(categoryQuestion);
        }
        [Authorize(Roles="A")]
        public ActionResult AdminConturol()
        {
            ViewBag.CountrolTest = Sql.ExecuteOne($@"Select * from QuestionsTest Where QStatus='Active' and AdminConturol=0 ");
            ViewBag.ControlOpen = Sql.ExecuteOne($@"Select * from QuestionsOpen Where QOStatus='Active' and AdminConturol=0 ");
            if (ViewBag.CountrolTest.Rows.Count==0 && ViewBag.ControlOpen.Rows.Count==0)
            {
                ViewBag.empty = "Təstiqlənəcək heçnə yoxdur...";
            }
            return View();
        }
        [HttpPost]
        public ActionResult AdminConturol(int[] QOId, int[] QId, Log log1)
        {
            Loglar log = new Loglar();
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            if (QOId != null)
            {
                for (int i = 0; i < QOId.Length; i++)
                {
                    QuestionsOpen questionsOpen = db.QuestionsOpens.Single(x => x.QOId == QOId[i]);
                    questionsOpen.AdminConturol = 1;
                    db.SubmitChanges();
                }
                log.UserId = Convert.ToInt32(userid);
                log.LogName = "Admin Accepted Open";
                log.LogDate = log1.LogDate;
                db.Loglars.InsertOnSubmit(log);
                db.SubmitChanges();
            }
            if (QId != null)
            {
                for (int i = 0; i < QId.Length; i++)
                {
                    QuestionsTest questionsTest = db.QuestionsTests.Single(x => x.QId == QId[i]);
                    questionsTest.AdminConturol = 1;
                    db.SubmitChanges();
                }
                //log.UserId = Convert.ToInt32(userid);
                //log.LogName = "Admin Accepted Test";
                //log.LogDate = log1.LogDate;
                //db.Loglars.InsertOnSubmit(log);
                //db.SubmitChanges();
            }
            return RedirectToAction(nameof(AdminConturol));
        }
        [Authorize(Roles ="A")]
        public ActionResult AdminProfile()
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.Teacher = Sql.ExecuteOne($@"Select UserName, UserFullName, UserEmail, UserDate, UserPhoto from Users Where UserId='" + Convert.ToInt32(userid) + "' ;");
            return View();
        }
        [HttpPost]
        public ActionResult AdminProfile(User user)
        {
            string userid = System.Web.HttpContext.Current.User.Identity.Name;
            User user1 = new User();
            User useredit = db.Users.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
            useredit.UserFullName = user.UserFullName;
            useredit.UserName = user.UserName;
            useredit.UserEmail = user.UserEmail;
            db.SubmitChanges();
            return RedirectToAction(nameof(AdminProfile));
        }
        [Authorize(Roles = "A")]
        public ActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNews(NewsClass news)
        {
            if (ModelState.IsValid)
            {
                New xbr = new New();
                
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + Request.Files[0].FileName);
                    string _path = Path.Combine(("~/Image"), _FileName);
                    Request.Files[0].SaveAs(HttpContext.Server.MapPath(_path));
                    xbr.NewsPhoto = _FileName;
                }
                xbr.NewsTitle = news.NewsTitle;
                xbr.NewsText = news.NewsText;
                xbr.NewsDate = news.NewsDate;
                db.News.InsertOnSubmit(xbr);
                db.SubmitChanges();
            }
            return RedirectToAction(nameof(AddNews));
        }
        
    }
}