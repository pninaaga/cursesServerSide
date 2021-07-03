using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Serverr.Model;
using Serverr.Controllers;

namespace Serverr.Controllers
{
    public class RegistrationsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();
        private Boolean openCours = false;
        // GET: api/Registrations
        public IQueryable GetRegistration()
        {
            return db.Registration.Select(x=>new {x.IdRegistration,x.IdCourse,x.RegistrationDate,x.StudentsTbl.FirstNameS,x.StudentsTbl.LastNameS,x.StudentsTbl.TzS, x.CoursesTbl.SubjectTbl.SubjectName });

        }

        // GET: api/Registrations/5
        [ResponseType(typeof(Registration))]
        public IHttpActionResult GetRegistration(short id)
        {
            Registration registration = db.Registration.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        // PUT: api/Registrations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegistration(short id, Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registration.IdRegistration)
            {
                return BadRequest();
            }

            db.Entry(registration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistrationExists(id))
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
        //public object GetIdSubjectAndIdTzS(string SubjectNameAndTzS)
        //{

        //}
        [Route("~/api/SendParamterToStudentRegistrations/{param?}")]
        [ResponseType(typeof(Registration))]
        public IHttpActionResult GetSendParamterToStudentRegistrations(string param)
        {
            int IdSubject = Convert.ToInt32(param.Split(' ')[0]);
            short DayInWeek =Convert.ToInt16(param.Split(' ')[1]);
            int IdConnect = Convert.ToInt32(param.Split(' ')[2]);
            var TzStudent = db.Connect.Find(IdConnect).Tz;//מוצא את התעודת זהות של התלמידה
            var RegIdStudent = db.StudentsTbl.Select(t => new { t.TzS, t.IdStudent }).FirstOrDefault(r => r.TzS == TzStudent).IdStudent;//לוקח את ה-id של התלמידה לפי התעודת זהות
            var idCourse = db.CoursesTbl.Select(y => new { y.DayInWeekC, y.IdCourse, y.IdSubject, y.OpenCourse }).Where(o => o.DayInWeekC == DayInWeek && o.IdSubject == IdSubject && o.OpenCourse == 0).ToList();
            Registration reg = new Registration();
            if (idCourse.Count() > 0)
            reg.IdCourse = idCourse[0].IdCourse; 
            reg.IdStudent = RegIdStudent;
            reg.RegistrationDate = DateTime.Today;
            openCours = true;
            this.PostRegistration(reg);
            return Ok(reg);
        } 

        // POST: api/Registrations
        [ResponseType(typeof(Registration))]
        public object PostRegistration(Registration registration)
            //string SubjectName, string TzS
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (registration is Registration)
            //{
            if (db.CoursesTbl.Find(registration.IdCourse).MaxStudents <= db.CoursesTbl.Find(registration.IdCourse).AmountStudentRegistration)
            {
                return -1;//תפוסה מלאה בקורס
            }
            //Registration reg = (registration as Registration);
            if (openCours == false) { 
            var TzStudent = db.Connect.Find(registration.IdStudent).Tz;//מוצא את התעודת זהות של התלמידה
            var RegIdStudent = db.StudentsTbl.Select(x => new { x.TzS, x.IdStudent }).FirstOrDefault(r => r.TzS == TzStudent).IdStudent;//לוקח את ה-id של התלמידה לפי התעודת זהות
            registration.IdStudent = RegIdStudent;
            }

            if (db.Registration.Select(x => new { x.IdCourse, x.IdStudent }).Where(e => e.IdStudent == registration.IdStudent && e.IdCourse == registration.IdCourse).Count() > 0)
                return -2;//כאשר התלמידה כבר נרשמה לקורס זה

            registration.RegistrationDate = DateTime.Today;
            //    registration.IdCourse = db.StudentsTbl.Max(x => x.IdStudent);
            //    var IdCourse = db.CoursesTbl.Select(x => new { x.IdCourse, x.IdSubject }).FirstOrDefault(p => p.IdSubject == registration.IdCourse);
            //    registration.IdCourse = IdCourse.IdCourse;
     
            short AmountSR = db.CoursesTbl.Find(registration.IdCourse).AmountStudentRegistration;
            db.CoursesTbl.Find(registration.IdCourse).AmountStudentRegistration = ++AmountSR;
            //db.CoursesTbl.Find(registration.IdCourse).AmountStudentRegistration = Convert.ToInt16(db.CoursesTbl.Find(registration.IdCourse).AmountStudentRegistration + 1);
            db.Registration.Add(registration);
               
                if (db.CoursesTbl.Select(x => new { x.IdCourse, x.MinStudents, x.AmountStudentRegistration }).Where(f => f.AmountStudentRegistration == f.MinStudents && f.IdCourse == registration.IdCourse).Count() > 0)//עכשיו יש מינימום נרשמות לקורס
                {
                    //this.GetLookForTeacher(registration.CoursesTbl.SubjectTbl.SubjectName);צריך לכתוב את זה רק במקרה שהקורס נפתח ע"י תלמידה 
                    db.CoursesTbl.Find(registration.IdCourse).OpenCourse = 1;

                }
           
                db.SaveChanges();
            openCours = false;
            return CreatedAtRoute("DefaultApi", new { id = registration.IdRegistration }, registration);

            //}

            //db.CoursesTbl.Select(p => p.AmountStudentRegistration);
            //= db.CoursesTbl.Select(p => p.AmountStudentRegistration + 1);


        }
        // DELETE: api/Registrations/5
        [ResponseType(typeof(Registration))]
        public IHttpActionResult DeleteRegistration(short id)
        {
            Registration registration = db.Registration.Find(id);
            if (registration == null)
            {
                return NotFound();
            }

            var AmountSR = db.CoursesTbl.Select(x => new { x.AmountStudentRegistration, x.IdCourse }).FirstOrDefault(y => y.IdCourse == registration.IdCourse);
            if (AmountSR.AmountStudentRegistration == db.CoursesTbl.Find(AmountSR.IdCourse).MaxStudents)
                return null;//במקרה שלא היה מקום לנרשמות חדשות כרגע אחת ירדה ומשהי מהממתינות יכולה להרשם
            if (AmountSR.AmountStudentRegistration == db.CoursesTbl.Find(AmountSR.IdCourse).MinStudents)
                db.CoursesTbl.Find(AmountSR.IdCourse).OpenCourse = 0; //במקרה והיה מינימום נרשמות, כרגע ירדה אחת אז הקורס עדיין לא נפתח 
            var n = AmountSR.AmountStudentRegistration - 1;
            db.CoursesTbl.Find(AmountSR.IdCourse).AmountStudentRegistration = Convert.ToInt16( n);                
            db.Registration.Remove(registration);
            db.SaveChanges();

            return Ok(registration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistrationExists(short id)
        {
            return db.Registration.Count(e => e.IdRegistration == id) > 0;
        }
        //[Route("~/api/LookForTeacher/{name}")]
        //public TeacherTbl GetLookForTeacher(string name)
        //{
        //    var max = 0;
        //    var day = 4;
        //    int Place = 0;
        //    TeacherTbl[] ListTeacher = new TeacherTbl[5];
        //    //  x.TeacherTbl.AvailableTeacherTbl.FirstOrDefault(s => s.DayInWeekT == da
        //    //var q1 = db.CoursesTbl.Select(x => new { x.IdSubject,x.SubjectTbl.SubjectName,x.TeacherTbl.FirstNameT,x.TeacherTbl.LastNameT, x.IdTeacher,x.TeacherTbl.Prefer}).Where(t => t.IdSubject == id);
        //    var q1 = db.CoursesTbl.Select(x => new { x.IdSubject, x.SubjectTbl.SubjectName, x.TeacherTbl.FirstNameT, x.TeacherTbl.LastNameT, x.IdTeacher, x.TeacherTbl.Prefer }).Where(t => t.SubjectName == name);

        //    var q2 = db.AvailableTeacherTbl.Select(t => new { t.IdTeacher, t.DayInWeekT }).Where(x => x.DayInWeekT == day);
        //    // var q3 = db.TeacherTbl.Select(x=> new { x.IdTeacher,q1,q2 }).Where(t=>t.q1.==5);//q1.Select(x => x.IdTeacher) == q2.Select(k => k.IdTeacher)
        //    var i = 0;
        //    foreach (var item in q2)
        //    {
        //        var q = q1.Select(x => new { x.IdTeacher, x.Prefer, x.FirstNameT, x.LastNameT }).Where(p => p.IdTeacher == item.IdTeacher).ToList();
        //        if (q.Count() != 0)
        //        {
        //            ListTeacher[i] = new TeacherTbl();
        //            ListTeacher[i].IdTeacher = Convert.ToInt16(q[0].IdTeacher);
        //            ListTeacher[i].Prefer = q[i].Prefer;
        //            ListTeacher[i].FirstNameT = q[i].FirstNameT;
        //            ListTeacher[i].LastNameT = q[i].LastNameT;
        //            i++;
        //        }
        //    }
        //    if (ListTeacher[0] != null)

        //        for (var j = 0; ListTeacher[j] != null; j++)
        //        {

        //            if (ListTeacher[j].Prefer > max)
        //            {
        //                max = ListTeacher[j].Prefer;
        //                Place = j;
        //            }
        //        }

        //    // for(i=0;i<ListTeacher.Length;i++)
        //    //    ListTeacher[i]
        //    // db.TeacherTbl.Select(x => new { x.Prefer, x.IdTeacher }).Where(p => p.IdTeacher == Convert.ToInt16(ListTeacher[i]));
        //    //var q4 = q1.Select(x => new { x.IdTeacher}).Where(f => f.IdTeacher.ToString() == q2.Select(r => r.IdTeacher).ToString());
        //    return ListTeacher[Place];
        //    // return db.TeacherTbl.Find(4);
        //    //(p => new { p.IdTeacher, q1, q2 }).FirstOrDefault(x => x.q1 == x.q2)
        //}
    }
}