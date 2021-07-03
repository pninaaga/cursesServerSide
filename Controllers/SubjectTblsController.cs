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
    public class SubjectTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/SubjectTbls
        public SubjectTbl[] GetSubjectTbl()
        {

            var lengthSubject = db.CoursesTbl.Select(x => x.IdCourse).Count();
            var q1 = db.CoursesTbl.Select(x => new { x.IdSubject, x.IdCourse });
            SubjectTbl[] ListSubject = new SubjectTbl[lengthSubject];
            var i = 1;
            var j = 0;
            bool flag = false;
            foreach (var item in q1)
            {
                var q2 = db.SubjectTbl.Select(x => new { x.IdSubject, x.IdFatherSubject, x.SubjectTbl2.SubjectName, x.Picture, x.CourseExplainText }).FirstOrDefault(y => y.IdSubject == item.IdSubject);
                if (ListSubject[0] == null)
                {
                    ListSubject[0] = new SubjectTbl();
                    ListSubject[0].SubjectName = q2.SubjectName;
                    ListSubject[0].IdFatherSubject = q2.IdFatherSubject;
                    ListSubject[0].Picture = q2.Picture;
                    ListSubject[0].CourseExplainText = q2.CourseExplainText;
                }
                else
                {
                    j = 0;
                    while (ListSubject[j] != null)
                    {
                        if (ListSubject[j].SubjectName == q2.SubjectName)
                        {
                            flag = true;
                            break;
                        }
                        j++;
                    }
                    if (flag == false)
                    {
                        ListSubject[i] = new SubjectTbl();
                        ListSubject[i].SubjectName = q2.SubjectName;
                        ListSubject[i].IdFatherSubject = q2.IdFatherSubject;
                        ListSubject[i].Picture = q2.Picture;
                        ListSubject[i].CourseExplainText = q2.CourseExplainText;
                        i++;
                    }
                    flag = false;

                }

            }

            return ListSubject;
        }
        [Route("~/api/GetAllSubjectsM")]
        [ResponseType(typeof(SubjectTbl))]
        public IQueryable GetAllSubjectsM()
        {
            return db.SubjectTbl.Select(x => new { x.IdSubject, x.SubjectName, x.IdFatherSubject, SubjectNameFather = x.SubjectTbl2.SubjectName, x.CourseExplainText, x.Picture });
        }

        // GET: api/SubSubjectTbls/5
        [Route("~/api/SubSubjectTbls/{id}")]
        //[ResponseType(typeof(SubjectTbl))]
        public object GetSubSubjectTbls(short id)

        {
            var q = db.SubjectTbl.Select(s => new { s.SubjectName, s.IdSubject, s.IdFatherSubject, s.Picture, s.CourseExplainText, priceAndAmountHour = s.CoursesTbl.Select(x => new { x.PricePerHour, x.AmountHour }) }).Where(s1 => s1.IdFatherSubject == id);
            var y = 0;
            var length = 0;
            var lengthSubSubject = q.Count();
            //var q1 = db.CoursesTbl.Select(x => new { x.IdSubject, x.SubjectTbl.SubjectName }).Where(p => p.IdSubject == ).ToList();
            foreach (var t in q)
            {
                length = db.CoursesTbl.Select(x => new { x.IdSubject, x.SubjectTbl.SubjectName }).Where(p => p.IdSubject == t.IdSubject).Count();
                y += length;
            }
            List<object> ListSubSubject = new List<object>();
            var i = 0;
            var j = 0;
            foreach (var item in q)
            {
                var q1 = db.CoursesTbl.Select(x => new { x.IdCourse, x.IdSubject, x.SubjectTbl.SubjectName, x.SubjectTbl.CourseExplainText,x.OpenCourse, x.SubjectTbl.Picture,x.AmountHour,x.PricePerHour }).Where(p => p.IdSubject == item.IdSubject).ToList();
                if (q1 != null)
                {
                    for (j = 0; j < q1.Count(); j++)
                    {
                        ListSubSubject.Add(q1[j]);
                        //ListSubSubject[i] = new object();
                        //ListSubSubject[i].SubjectName = q1[j].SubjectName;
                        //ListSubSubject[i].IdSubject = q1[j].IdSubject;
                        //ListSubSubject[i]. = q1[j].IdSubject;

                        i++;
                    }
                }

            }

            return ListSubSubject;
        }
        // GET: api/AllSubSubjectTbls
        [Route("~/api/AllSubSubjectTbls")]
        //[ResponseType(typeof(SubjectTbl))]
        public IQueryable GetAllSubSubjectTbls()
        {
            return db.SubjectTbl.Select(s => new { s.SubjectName, s.IdSubject, s.IdFatherSubject, s.Picture, s.CourseExplainText}).Where(s1 => s1.IdFatherSubject != null);
        }

        // GET: api/SubjectTbls/5
        [ResponseType(typeof(SubjectTbl))]
        public IHttpActionResult GetSubjectTbl(short id)
        {
            SubjectTbl subjectTbl = db.SubjectTbl.FirstOrDefault(s1 => s1.IdFatherSubject == id);
            if (subjectTbl == null)
            {
                return NotFound();
            }

            return Ok(subjectTbl);
        }
        [Route("~/api/SubjectTblsForManager/{id}")]
        [ResponseType(typeof(SubjectTbl))]
        public IHttpActionResult GetSubjectTblforManager(short id)
        {
            //SubjectTbl subjectTbl = db.SubjectTbl.Find(id);
            var subjectTbl = db.SubjectTbl.Select(x => new { x.IdSubject, x.SubjectName, x.Picture, x.CourseExplainText, SubjectNameFather = x.SubjectTbl2.SubjectName }).Where(s => s.IdSubject == id);

            if (subjectTbl == null)
            {
                return NotFound();
            }

            return Ok(subjectTbl);
        }



        // PUT: api/SubjectTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubjectTbl(short id, SubjectTbl subjectTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subjectTbl.IdSubject)
            {
                return BadRequest();
            }

            db.Entry(subjectTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectTblExists(id))
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

        // POST: api/SubjectTbls
        [ResponseType(typeof(SubjectTbl))]
        public IHttpActionResult PostSubjectTbl(SubjectTbl subjectTbl)
        {
            subjectTbl.IdSubject = Convert.ToInt16(db.SubjectTbl.Select(x => x.IdSubject).Max() + 1);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SubjectTbl.Add(subjectTbl);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SubjectTblExists(subjectTbl.IdSubject))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = subjectTbl.IdSubject }, subjectTbl);
        }

        // DELETE: api/SubjectTbls/5
        [ResponseType(typeof(SubjectTbl))]
        public IHttpActionResult DeleteSubjectTbl(short id)
        {
            SubjectTbl subjectTbl = db.SubjectTbl.Find(id);
            if (subjectTbl == null)
            {
                return NotFound();
            }

            db.SubjectTbl.Remove(subjectTbl);
            db.SaveChanges();

            return Ok(subjectTbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectTblExists(short id)
        {
            return db.SubjectTbl.Count(e => e.IdSubject == id) > 0;
        }
    }
}