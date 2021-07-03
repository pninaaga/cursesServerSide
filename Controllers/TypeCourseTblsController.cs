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
    public class TypeCourseTblsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/TypeCourseTbls
        public IQueryable<TypeCourseTbl> GetTypeCourseTbl()
        {
            return db.TypeCourseTbl;
        }

        // GET: api/TypeCourseTbls/5
        [ResponseType(typeof(TypeCourseTbl))]
        public IHttpActionResult GetTypeCourseTbl(short id)
        {
            TypeCourseTbl typeCourseTbl = db.TypeCourseTbl.Find(id);
            if (typeCourseTbl == null)
            {
                return NotFound();
            }

            return Ok(typeCourseTbl);
        }

        // PUT: api/TypeCourseTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTypeCourseTbl(short id, TypeCourseTbl typeCourseTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeCourseTbl.IdTypeCourse)
            {
                return BadRequest();
            }

            db.Entry(typeCourseTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeCourseTblExists(id))
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

        // POST: api/TypeCourseTbls
        [ResponseType(typeof(TypeCourseTbl))]
        public IHttpActionResult PostTypeCourseTbl(TypeCourseTbl typeCourseTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypeCourseTbl.Add(typeCourseTbl);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = typeCourseTbl.IdTypeCourse }, typeCourseTbl);
        }

        // DELETE: api/TypeCourseTbls/5
        [ResponseType(typeof(TypeCourseTbl))]
        public IHttpActionResult DeleteTypeCourseTbl(short id)
        {
            TypeCourseTbl typeCourseTbl = db.TypeCourseTbl.Find(id);
            if (typeCourseTbl == null)
            {
                return NotFound();
            }

            db.TypeCourseTbl.Remove(typeCourseTbl);
            db.SaveChanges();

            return Ok(typeCourseTbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeCourseTblExists(short id)
        {
            return db.TypeCourseTbl.Count(e => e.IdTypeCourse == id) > 0;
        }
    }
}