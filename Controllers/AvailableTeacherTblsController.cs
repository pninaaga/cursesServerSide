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
    public class AvailableTeacherTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/AvailableTeacherTbls
        public IQueryable<AvailableTeacherTbl> GetAvailableTeacherTbl()
        {
            return db.AvailableTeacherTbl;
        }

        // GET: api/AvailableTeacherTbls/5
        [ResponseType(typeof(AvailableTeacherTbl))]
        public IHttpActionResult GetAvailableTeacherTbl(short id)
        {
            AvailableTeacherTbl availableTeacherTbl = db.AvailableTeacherTbl.Find(id);
            if (availableTeacherTbl == null)
            {
                return NotFound();
            }

            return Ok(availableTeacherTbl);
        }

        // PUT: api/AvailableTeacherTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAvailableTeacherTbl(short id, AvailableTeacherTbl availableTeacherTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != availableTeacherTbl.IdAvailable)
            {
                return BadRequest();
            }

            db.Entry(availableTeacherTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailableTeacherTblExists(id))
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

        // POST: api/AvailableTeacherTbls
        [ResponseType(typeof(AvailableTeacherTbl))]
        public IHttpActionResult PostAvailableTeacherTbl(int[] Days, [FromUri] string Tz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AvailableTeacherTbl AvaliableTeacher = new AvailableTeacherTbl();
            for (var i=0; i < Days.Length; i++) {
                AvaliableTeacher.DayInWeekT =Convert.ToInt16( Days[i]);
                
                var temp = db.TeacherTbl.Select(x => new { x.IdTeacher, x.TzT }).FirstOrDefault(y => y.TzT == Tz);
                if (temp !=null)
                    AvaliableTeacher.IdTeacher =Convert.ToInt16(temp.IdTeacher);
                db.AvailableTeacherTbl.Add(AvaliableTeacher);
                db.SaveChanges();
            }

           

            return CreatedAtRoute("DefaultApi", new { id = AvaliableTeacher.IdAvailable }, AvaliableTeacher);
        }

        // DELETE: api/AvailableTeacherTbls/5
        [ResponseType(typeof(AvailableTeacherTbl))]
        public IHttpActionResult DeleteAvailableTeacherTbl(short id)
        {
            AvailableTeacherTbl availableTeacherTbl = db.AvailableTeacherTbl.Find(id);
            if (availableTeacherTbl == null)
            {
                return NotFound();
            }

            db.AvailableTeacherTbl.Remove(availableTeacherTbl);
            db.SaveChanges();

            return Ok(availableTeacherTbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AvailableTeacherTblExists(short id)
        {
            return db.AvailableTeacherTbl.Count(e => e.IdAvailable == id) > 0;
        }
    }
}