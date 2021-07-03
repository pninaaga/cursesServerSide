using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using Serverr.Model;


namespace Serverr.Controllers
{
    public class CoursesTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();


        //public IQueryable GetCoursesTbl()
        //{
        //    return db.CoursesTbl.Select(x => new { x.AmountHour, x.DayInWeekC, x.FromHourC, x.UntilHourC, x.MaxStudents, x.MinStudents, x.OpenCourse, x.PricePerHour });/*,x.SubjectTbl.SubjectName,x.TeacherTbl.FirstNameT });*/

        //}
        // GET: api/CoursesTbls
        public IQueryable GetCoursesTbl()
        {
             return db.CoursesTbl.Select(x=>new { x.AmountHour, x.AmountStudentRegistration, x.DayInWeekC, x.FromHourC, x.IdCourse, x.SubjectTbl.SubjectName,x.SubjectTbl.IdSubject, x.TeacherTbl.FirstNameT, x.TeacherTbl.LastNameT, x.IdTypeCourse, x.MaxStudents, x.MinStudents, x.OpenCourse, x.PricePerHour,x.UntilHourC });
            
        }

        // GET: api/CoursesTbls/5
        [ResponseType(typeof(CoursesTbl))]
        public IHttpActionResult GetCoursesTbl(short id)
        
            {
           
            CoursesTbl coursesTbl = db.CoursesTbl.Find(id);
            var c = db.CoursesTbl.Select(x => new { x.AmountHour, x.AmountStudentRegistration, x.DayInWeekC, x.FromHourC, x.IdCourse, x.SubjectTbl.SubjectName, x.TeacherTbl.FirstNameT,x.TeacherTbl.LastNameT,x.TeacherTbl.TzT, x.IdTypeCourse, x.MaxStudents, x.MinStudents, x.OpenCourse, x.PricePerHour, x.UntilHourC }).FirstOrDefault(r => r.IdCourse == id);
           ;

            if (c == null)
            {
                return NotFound();
            }

            return Ok(c);
        }
        // GET: api/CoursesTblsBySubjectId/5
        [Route("~/api/CoursesTblsBySubjectId/{id}")]
        //[ResponseType(typeof(CoursesTbl))]
        public object GetCoursesTblsBySubjectId(short id)
        {
            //var q = db.CoursesTbl.Select(x =>new { x.IdSubject, x.IdCourse }).FirstOrDefault(c=>c.IdSubject==id);
            //CoursesTbl coursesTbl = db.CoursesTbl.Find(q.IdCourse);
            CoursesTbl coursesTbl = db.CoursesTbl.Find(id);
            if (coursesTbl == null)
            {
                return null;
            }
            return db.CoursesTbl.Select(x=>new { x.FromHourC, x.UntilHourC, x.IdCourse, x.DayInWeekC, x.AmountHour, x.MaxStudents, x.MinStudents, x.PricePerHour }).FirstOrDefault(y=>y.IdCourse==id);
        }

        //[Route("~/api/CoursesTblsDetails/{id}")]
        //// GET: api/CoursesTblsDetails/5
        //[ResponseType(typeof(CoursesTbl))]
        //public IHttpActionResult GetCoursesTblDetails(short id)
        //{
        //    var Id = db.CoursesTbl.Select(x => new { x.IdCourse, x.IdSubject }).Where(p => p.IdSubject == id);
        //    var idDetails = Id.Select(y => y.IdCourse);
        //    CoursesTbl coursesTbl = db.CoursesTbl.FirstOrDefault(x=>x.IdCourse==Convert.ToInt16(idDetails));
        //    if (coursesTbl == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(coursesTbl);
        //}

        [Route("~/api/MinStudent/{Subject}")]
        [ResponseType(typeof(CoursesTbl))]
        public IQueryable GetMinStudent(string Subject)
        {
            var q = db.SubjectTbl.Select(x => new { x.IdSubject, x.SubjectName }).Where(t => t.SubjectName == Subject);
            var IdSubject = q.Select(u => u.IdSubject);
            if (IdSubject == null)
            {
                return IdSubject;
            }
            var q1 = db.CoursesTbl.Select(x => new { x.IdSubject,x.MinStudents }).Where(t =>t.IdSubject == Convert.ToInt16(IdSubject));
            //var q1= db.CoursesTbl.Select(x => new { x.IdSubject, x.MinStudents }).Where(r => r.IdSubject.ToString() == IdSubject.ToString());
            return q1.Select(x => x.MinStudents);
        }

        // PUT: api/CoursesTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCoursesTbl(short id, CoursesTbl coursesTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coursesTbl.IdCourse)
            {
                return BadRequest();
            }

            db.Entry(coursesTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursesTblExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/CoursesTbls
        [ResponseType(typeof(CoursesTbl))]
        public IHttpActionResult PostCoursesTbl(CoursesTbl coursesTbl)
        {
            if (coursesTbl.IdTeacher == null)
               coursesTbl.IdTeacher= db.TeacherTbl.Select(x => new { x.IdTeacher, x.FirstNameT, x.LastNameT }).FirstOrDefault(y => y.LastNameT == coursesTbl.TeacherTbl.LastNameT && y.FirstNameT == coursesTbl.TeacherTbl.FirstNameT).IdTeacher;
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.CoursesTbl.Add(coursesTbl);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = 5 }, coursesTbl);
        }
        // POST: api/PostCourseWithNewSubjects
        [Route("~/api/CourseWithNewSubjects")]
        [ResponseType(typeof(void))]
        public void PostCourseWithNewSubjects(JObject Data)
            //string TzT, [FromBody] string SubjectName, [FromBody]string FatherSubject, [FromBody]CoursesTbl Course
        {

           var Tz= Data["TzT"].Value<string>();
            //if (Data.TzT is CoursesTbl)
            //{
            //    CoursesTbl NewCourse = (Data as CoursesTbl);

            //}


            //var IdTeacher = db.TeacherTbl.Select(x => new { x.IdTeacher, x.TzT }).Where(y => y.TzT == Data.);
            var NewCourse = new CoursesTbl();
            //NewCourse = Data.ModelC;

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //db.CoursesTbl.Add(Data);
            //db.SaveChanges();
            //PostCoursesTbl(Course);
            //return CreatedAtRoute("DefaultApi", new { id = 5 }, Data);
        }

        [Route("~/api/CreateOpenCourse")]
        [ResponseType(typeof(CoursesTbl))]
        public IHttpActionResult PostCreateOpenCourse(CoursesTbl coursesTbl)
        {
            var IdpreviousCourse = db.CoursesTbl
                .Select(x => new { x.IdCourse, x.IdSubject })
                .FirstOrDefault(t => t.IdSubject == coursesTbl.IdSubject);
            var previousCourse = db.CoursesTbl.Find(IdpreviousCourse.IdCourse);
            coursesTbl.FromHourC = previousCourse.FromHourC;
            coursesTbl.OpenCourse = 0;
            coursesTbl.UntilHourC = previousCourse.UntilHourC;
            coursesTbl.AmountHour = previousCourse.AmountHour;
            coursesTbl.MinStudents = previousCourse.MinStudents;
            coursesTbl.AmountStudentRegistration = 0;
           short idTeacher= LookForTeacher(coursesTbl.IdSubject,coursesTbl.DayInWeekC);
            coursesTbl.IdTeacher = idTeacher;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.CoursesTbl.Add(coursesTbl);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = coursesTbl.IdCourse }, coursesTbl);

        }

        // DELETE: api/CoursesTbls/5
        [ResponseType(typeof(CoursesTbl))]
        public IHttpActionResult DeleteCoursesTbl(short id)
        {
            CoursesTbl coursesTbl = db.CoursesTbl.Find(id);
            if (coursesTbl == null)
            {
                return NotFound();
            }

            db.CoursesTbl.Remove(coursesTbl);
            db.SaveChanges();

            return Ok(coursesTbl);
        }


        [Route("~/api/GetDetailsForOpenCoursComponent/{subjectNameAndDay?}")]
        [ResponseType(typeof(CoursesTbl))]
        public object GetDetailsForOpenCoursComponent(string subjectNameAndDay)
        {
            string subjectName; 
            Int16 day;
            int countSpace = 0, i,j;
            for ( i = 0; i < subjectNameAndDay.Length; i++)
                if (subjectNameAndDay[i] == ' ')
                    countSpace++;

            if(countSpace==1)//שם הקורס מכיל מילה אחת
            { 
             subjectName =Convert.ToString(subjectNameAndDay.Split(' ')[0]);
             day = Convert.ToInt16(subjectNameAndDay.Split(' ')[1]);
            }
            else//שם הקורס מכיל יותר ממילה אחת
            {
                subjectName = subjectNameAndDay.Split(' ')[0];
                for ( j = 1; j < countSpace; j++)
                    subjectName+=" "+ subjectNameAndDay.Split(' ')[j];
                day = Convert.ToInt16(subjectNameAndDay.Split(' ')[j]);

            }
            var temp = db.CoursesTbl.Select(x=>new {x.PricePerHour,x.AmountHour, x.IdCourse,x.SubjectTbl.SubjectName,x.DayInWeekC,x.MaxStudents,x.AmountStudentRegistration }).Where(v => v.SubjectName == subjectName).ToList();//שולף את הפרטים של הקורסים שיש להם את אותו שם

            //var temp = db.CoursesTbl.Select(x => new { x.SubjectTbl.SubjectName, x.IdSubject, x.PricePerHour, x.AmountHour, x.MinStudents }).Where(v => v.SubjectName == subjectName).ToList();
            if (temp.Count() > 0)
            {
                if (temp.Select(x => new { x.DayInWeekC, x.MaxStudents, x.AmountStudentRegistration }).Where(v => v.DayInWeekC == day && v.MaxStudents > v.AmountStudentRegistration).Count() > 0)//האם מה ששלפנו-טמפ, היום של הקורס הוא שווה ליום שהתלמידה רוצה לפתוח את הקורס וכן שיש מקום עוד להירשם לקורס זה 
                    return -1;//התלמידה לא יכולה לפתוח את הקורס כי כבר יש קורס זה ביום זה שיש בו מקום להרשם
               // var d = db.CoursesTbl.Find(temp[0].IdCourse);
                return temp[0];//מחזיר פרטים של הקורס הראשון שמצא עם אותו שם
                //return temp[0]
            }
            else//לא מצא שם קורס כזה ברשימת הקורסים 
                if (db.SubjectTbl.Select(x => new { x.IdSubject, x.SubjectName }).Where(r => r.SubjectName == subjectName).Count() > 0)//אם שם הקורס קיים ברשימת נושאי הקורסים
                sendDetailsCoursManagerEmail(subjectName);
            else
            sendNewCoursManagerEmail(subjectName);//כאשר נושא הקורס חדש

            return 0;

            //var temp = db.CoursesTbl.Select(x => new { x.IdTeacher, x.TeacherTbl.Price, x.AmountHour, x.MinStudents }).FirstOrDefault(p => p.IdTeacher == id);
        }
        public static void sendNewCoursManagerEmail(string subjectName)
        {
            var message = new MailMessage();
            message.From = new MailAddress("sitefurthertraining@gmail.com");
            message.To.Add(new MailAddress("pnina15461@gmail.com"));
            message.Subject = "בקשה לפתיחת קורס";
            message.Body += " נפתחה קריאה לפתיחת קורס בנושא";
            message.Body += subjectName;
            message.Body += "שאינו קיים במערכת ";
            //+Environment.NewLine;
            message.Body += "אנא אשר על ידי כניסה למערכת ויצירת קורס זה";
                message.Body+= Environment.NewLine;
            message.Body = " http://localhost:4200/   לכניסה לאתר";

            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;
            var client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(message.From.User, "0533137519");
            client.Host = "smtp.gmail.com";
            client.Send(message);
        }
        public static void sendDetailsCoursManagerEmail(string subjectName)
        {
            var message = new MailMessage();
            message.From = new MailAddress("sitefurthertraining@gmail.com");
            message.To.Add(new MailAddress("pnina15461@gmail.com"));
            message.Subject = " בקשה לפרטים לפתיחת קורס בנושא קיים במערכת";
            message.Body += " נפתחה קריאה לפתיחת קורס";
            message.Body += subjectName+Environment.NewLine;
            message.Body += "אנא כנס לאתר ומלא את הפרטים הנדרשים (צור קורס חדש) על מנת שהקורס יוכל ליפתח" + Environment.NewLine;
            message.Body = " http://localhost:4200/   :לכניסה לאתר";
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;
            var client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(message.From.User, "0533137519");
            client.Host = "smtp.gmail.com";
            client.Send(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CoursesTblExists(short id)
        {
            return db.CoursesTbl.Count(e => e.IdCourse == id) > 0;
        }


        // Own Actions
        [Route("aa")]
        public IQueryable GetCoursesOwnn(short id)
        {
            int Id = id;
            return db.CoursesTbl.Select(x => new { x.AmountHour, x.DayInWeekC, x.FromHourC, x.UntilHourC, x.MaxStudents, x.MinStudents, x.OpenCourse, x.PricePerHour });/*,x.SubjectTbl.SubjectName,x.TeacherTbl.FirstNameT });*/
        }
        //[Route("~/api/")]
        //public IQueryable GetCoursesOwnn(object id)
        //{

        //   var e= db.CoursesTbl.Contains(id);
        //    db.CoursesTbl.
        //   //var w= cont(db.CoursesTbl, id);
        //   // db.CoursesTbl.Select(x => new { x.IdCourse }).Where(t => t.IdCourse);
        //   // return db.CoursesTbl.Select(x => new { x.AmountHour, x.DayInWeekC, x.FromHourC, x.UntilHourC, x.MaxStudents, x.MinStudents, x.OpenCourse, x.PricePerHour });
        //}




        //[Route("~/api/LookForTeacher/{name}")]
        public short LookForTeacher(short idSubject, short day)
        {
            //var max = 0;
            //var day = 4;
            //int Place = 0;
            //TeacherTbl[] ListTeacher = new TeacherTbl[5];
            //  x.TeacherTbl.AvailableTeacherTbl.FirstOrDefault(s => s.DayInWeekT == da
            //var q1 = db.CoursesTbl.Select(x => new { x.IdSubject,x.SubjectTbl.SubjectName,x.TeacherTbl.FirstNameT,x.TeacherTbl.LastNameT, x.IdTeacher,x.TeacherTbl.Prefer}).Where(t => t.IdSubject == id);
            //var ListTeacher = db.CoursesTbl.Select(x => new { x.IdSubject, x.SubjectTbl.SubjectName, x.TeacherTbl.FirstNameT, x.TeacherTbl.LastNameT, x.IdTeacher }).ToList();
            var ListTeacher = db.SubjectOfTeacherTbl.Select(x => new { x.IdSubject, x.IdTeacher,x.TeacherTbl.Prefer, Day = x.TeacherTbl.AvailableTeacherTbl.Select(o => o.DayInWeekT)}).Where(t => t.IdSubject == idSubject && t.Day.Contains(day)).OrderBy(u=>u.Prefer).Take(1);
            //Day = x.TeacherTbl.AvailableTeacherTbl.Select(o => o.DayInWeekT)
            //var q2 = db.AvailableTeacherTbl.Select(t => new { t.IdTeacher, t.DayInWeekT }).Where(x => x.DayInWeekT == day);
            // var q3 = db.TeacherTbl.Select(x=> new { x.IdTeacher,q1,q2 }).Where(t=>t.q1.==5);//q1.Select(x => x.IdTeacher) == q2.Select(k => k.IdTeacher)
            //var i = 0;
            //foreach (var item in q2)
            //{
            //    var q = q1.Select(x => new { x.IdTeacher, x.Prefer, x.FirstNameT, x.LastNameT }).Where(p => p.IdTeacher == item.IdTeacher).ToList();
            //    if (q.Count() != 0)
            //    {
            //        ListTeacher[i] = new TeacherTbl();
            //        ListTeacher[i].IdTeacher = Convert.ToInt16(q[0].IdTeacher);
            //        ListTeacher[i].Prefer = q[i].Prefer;
            //        ListTeacher[i].FirstNameT = q[i].FirstNameT;
            //        ListTeacher[i].LastNameT = q[i].LastNameT;
            //        i++;
            //    }
            //}
            //if (ListTeacher[0] != null)

            //    for (var j = 0; ListTeacher[j] != null; j++)
            //    {

            //        if (ListTeacher[j].Prefer > max)
            //        {
            //            max = ListTeacher[j].Prefer;
            //            Place = j;
            //        }
            //    }

            // for(i=0;i<ListTeacher.Length;i++)
            //    ListTeacher[i]
            // db.TeacherTbl.Select(x => new { x.Prefer, x.IdTeacher }).Where(p => p.IdTeacher == Convert.ToInt16(ListTeacher[i]));
            //var q4 = q1.Select(x => new { x.IdTeacher}).Where(f => f.IdTeacher.ToString() == q2.Select(r => r.IdTeacher).ToString());
            return ListTeacher.ToList()[0].IdTeacher;
            // return db.TeacherTbl.Find(4);
            //(p => new { p.IdTeacher, q1, q2 }).FirstOrDefault(x => x.q1 == x.q2)
        }

    }
}


