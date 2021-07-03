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
    public class SubjectOfTeacherTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/SubjectOfTeacherTbls
        public IQueryable<SubjectOfTeacherTbl> GetSubjectOfTeacherTbl()
        {
            return db.SubjectOfTeacherTbl;
        }

        // GET: api/SubjectOfTeacherTbls/5
        [ResponseType(typeof(SubjectOfTeacherTbl))]
        public IHttpActionResult GetSubjectOfTeacherTbl(short id)
        {
            SubjectOfTeacherTbl subjectOfTeacherTbl = db.SubjectOfTeacherTbl.Find(id);
            if (subjectOfTeacherTbl == null)
            {
                return NotFound();
            }

            return Ok(subjectOfTeacherTbl);
        }

        // PUT: api/SubjectOfTeacherTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubjectOfTeacherTbl(short id, SubjectOfTeacherTbl subjectOfTeacherTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subjectOfTeacherTbl.IdSubjectOfTeacher)
            {
                return BadRequest();
            }

            db.Entry(subjectOfTeacherTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectOfTeacherTblExists(id))
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

        // POST: api/SubjectOfTeacherTbls
        [ResponseType(typeof(SubjectOfTeacherTbl))]
        public IHttpActionResult PostSubjectOfTeacherTbl(int[] subjectOfTeacherTbl ,[FromUri] string Tz)
        {
            SubjectOfTeacherTbl SubjectOfTeacher = new SubjectOfTeacherTbl();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            for(var i=0;i<subjectOfTeacherTbl.Length; i++)
            {
                SubjectOfTeacher.IdSubject =Convert.ToInt16(subjectOfTeacherTbl[i]);
                SubjectOfTeacher.IdTeacher = db.TeacherTbl.Select(x => new { x.TzT, x.IdTeacher }).FirstOrDefault(y => y.TzT == Tz).IdTeacher;
                db.SubjectOfTeacherTbl.Add(SubjectOfTeacher);
                db.SaveChanges();
            }
            
        

            return CreatedAtRoute("DefaultApi", new { id = SubjectOfTeacher.IdSubjectOfTeacher }, SubjectOfTeacher);
        }

        // DELETE: api/SubjectOfTeacherTbls/5
        [ResponseType(typeof(SubjectOfTeacherTbl))]
        public IHttpActionResult DeleteSubjectOfTeacherTbl(short id)
        {
            SubjectOfTeacherTbl subjectOfTeacherTbl = db.SubjectOfTeacherTbl.Find(id);
            if (subjectOfTeacherTbl == null)
            {
                return NotFound();
            }

            db.SubjectOfTeacherTbl.Remove(subjectOfTeacherTbl);
            db.SaveChanges();

            return Ok(subjectOfTeacherTbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectOfTeacherTblExists(short id)
        {
            return db.SubjectOfTeacherTbl.Count(e => e.IdSubjectOfTeacher == id) > 0;
        }
    }
}