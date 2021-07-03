using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using System.Web.Http.Description;
using Serverr.Model;


namespace Serverr.Controllers
{

    
    public class StudentsTblsController : ApiController
    {
        public  short idStudentR;
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();
        
        // GET: api/StudentsTbls
        public IQueryable GetStudentsTbl()
        {
            return db.StudentsTbl.Select(x=>new {x.IdStudent,x.AddressS,x.EmailS,x.FirstNameS,x.LastNameS,x.PhoneS,x.TzS });
        }

        // GET: api/StudentsTbls/5
        [ResponseType(typeof(StudentsTbl))]
        public IHttpActionResult GetStudentsTbl(short id)
        {
            StudentsTbl studentsTbl = db.StudentsTbl.Find(id);
            var S = db.StudentsTbl.Select(x => new { x.IdStudent, x.FirstNameS, x.LastNameS, x.AddressS, x.PhoneS, x.EmailS, x.TzS }).FirstOrDefault(y => y.IdStudent == id);

            if (S == null)
            {
                return NotFound();
            }

            return Ok(S);
        }

        // PUT: api/StudentsTbls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudentsTbl(short id, StudentsTbl studentsTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentsTbl.IdStudent)
            {
                return BadRequest();
            }

            db.Entry(studentsTbl).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsTblExists(id))
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

        // POST: api/StudentsTbls
        [ResponseType(typeof(StudentsTbl))]
        public IHttpActionResult PostStudentsTbl(StudentsTbl studentsTbl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var NewStudent = db.StudentsTbl.Select(x => x.TzS).Where(n => n == studentsTbl.TzS).Count();
                if (NewStudent == 0)
            { 
            db.StudentsTbl.Add(studentsTbl);
            db.SaveChanges();
            }
            //var b = db.StudentsTbl.ToList();
            //var y = 7;
            //idStudentR = Convert.ToInt16(new { id = studentsTbl.IdStudent });

            return CreatedAtRoute("DefaultApi", new { id = studentsTbl.IdStudent }, studentsTbl);
        }

        // DELETE: api/StudentsTbls/5
        [ResponseType(typeof(StudentsTbl))]
        public IHttpActionResult DeleteStudentsTbl(short id)
        {
            StudentsTbl studentsTbl = db.StudentsTbl.Find(id);
            if (studentsTbl == null)
            {
                return NotFound();
            }

            db.StudentsTbl.Remove(studentsTbl);
            db.SaveChanges();

            return Ok(studentsTbl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentsTblExists(short id)
        {
            return db.StudentsTbl.Count(e => e.IdStudent == id) > 0;
        }
        [ResponseType(typeof(StudentsTbl))]
        [Route("~/api/Getcontact/{detailsContact?}")]
        public void Getcontact(string detailsContact )
        {
            var message = new MailMessage();
            message.From = new MailAddress("sitefurthertraining@gmail.com");
            message.To.Add(new MailAddress("chevi7519@gmail.com"));
            message.Subject = detailsContact.Split(' ')[0];
            message.Body += detailsContact.Split(' ')[1];
            message.Body += detailsContact.Split(' ')[2]+Environment.NewLine;
            message.Body += detailsContact.Split(' ')[3] + Environment.NewLine;
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
    }
}