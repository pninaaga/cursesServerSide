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

namespace Serverr.Controllers
{
    public class TeacherTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/TeacherTbls
        public IQueryable GetTeacherTbl()
        {
         //   return db.TeacherTbl;
            return db.TeacherTbl.Select(x=>new { x.IdTeacher, x.FirstNameT, x.LastNameT, x.AddressT, x.PhoneT, x.EmailT, x.TzT, x.Price, x.Prefer, AvaliableList = x.AvailableTeacherTbl.Select(u =>u.DayInWeekT),SubjectList=x.SubjectOfTeacherTbl.Select(d=>new { d.SubjectTbl.SubjectName, d.IdSubject })});
        }

        // GET: api/TeacherTbls/5
        [ResponseType(typeof(TeacherTbl))]
        public IHttpActionResult GetTeacherTbl(short id)
        {
            TeacherTbl teacherTbl = db.TeacherTbl.Find(id);
            var T=db.TeacherTbl.Select(x => new { x.IdTeacher, x.FirstNameT, x.LastNameT, x.AddressT, x.PhoneT, x.EmailT, x.TzT, x.Price, x.Prefer, SubjectListTeacher=x.SubjectOfTeacherTbl.Select(r=>new { r.SubjectTbl.SubjectName, r.IdSubject }), AvaliableListTeacher=x.AvailableTeacherTbl.Select(o=>o.DayInWeekT) }).FirstOrDefault(y=>y.IdTeacher==id);

            if (T == null)
            {
                return NotFound();
            }
            //T.SubjectOfTeacherTbl.Select(x => new { x.IdSubject, x.SubjectTbl.SubjectName });
            //T.AvailableTeacherTbl.Select(y => y.DayInWeekT);
            return Ok(T);
        }
        [Route("~/api/GetSubjects/{id}")]
        [ResponseType(typeof(TeacherTbl))]
        public object GetSubjects(int id)
            {
        
            List<object> TeachersList = new List<object>();
            List<object> SubjectsList = new List<object>();

            var q1 = db.TeacherTbl.Select(x => new { x.IdTeacher, x.FirstNameT, x.LastNameT, x.TzT});
            if (id == -1)
            {
                foreach (var item1 in q1)
                {
                    var Teacher = db.SubjectOfTeacherTbl.Select(y => new { y.IdSubject, y.IdTeacher,item1.LastNameT,item1.FirstNameT,item1.TzT }).Where(c => c.IdTeacher == item1.IdTeacher);
                    if (Teacher.Count() > 0)
                        TeachersList.Add(Teacher);
                }
            }
            else
            foreach (var item in q1)
            {
                var Teacher = db.SubjectOfTeacherTbl.Select(y => new { y.IdSubject, y.IdTeacher }).Where(c => c.IdTeacher == item.IdTeacher && c.IdSubject==id);
                if(Teacher.Count()>0)
                TeachersList.Add(item);
            }
            List<object> SubjectAndTeachersList = new List<object>();
            SubjectAndTeachersList.Add(SubjectsList);
            SubjectAndTeachersList.Add(TeachersList);
            SubjectsList.Add(db.SubjectTbl.Select(t => new { t.IdSubject, t.SubjectName, t.IdFatherSubject }).Where(d => d.IdFatherSubject != null));
            return SubjectAndTeachersList;

            //return db.TeacherTbl.Select(x => new {x.IdTeacher, x.FirstNameT, x.LastNameT,x.TzT });
        }


        // PUT: api/TeacherTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacherTbl(short id, TeacherTbl teacherTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacherTbl.IdTeacher)
            {
                return BadRequest();
            }

            db.Entry(teacherTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherTblExists(id))
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

        // POST: api/TeacherTbls
        [ResponseType(typeof(TeacherTbl))]
        public IHttpActionResult PostTeacherTbl(TeacherTbl teacherTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var t = db.TeacherTbl.Select(t2=>new { t2.TzT }).FirstOrDefault(t1=>t1.TzT==teacherTbl.TzT);
            if (t == null)
            {
                db.TeacherTbl.Add(teacherTbl);
                db.SaveChanges();
            }
                

            return CreatedAtRoute("DefaultApi", new { id = teacherTbl.IdTeacher }, teacherTbl);
        }

        // DELETE: api/TeacherTbls/5
        [ResponseType(typeof(TeacherTbl))]
        public object DeleteTeacherTbl(short id)
        {
            TeacherTbl teacherTbl = db.TeacherTbl.Find(id);
            if (teacherTbl == null)
            {
                return NotFound();
            }
            //if (db.CoursesTbl.Select(t => t.IdTeacher == id).Count() > 0)טעות
                if (db.CoursesTbl.Select(t =>new { t.IdTeacher }).Where(x=>x.IdTeacher==id).Count() > 0)
                return null;
            db.TeacherTbl.Remove(teacherTbl);
           
            db.SaveChanges();

            return Ok(teacherTbl);
        }
        //חיפוש מורה
        //api/LookForTeacher/5
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherTblExists(short id)
        {
            return db.TeacherTbl.Count(e => e.IdTeacher == id) > 0;
        }
    }
}