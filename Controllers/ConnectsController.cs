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
using Serverr.Model;

namespace Serverr.Controllers
{
    public class ConnectsController : ApiController
    {
        private FurtherTraining3Entities4 db = new FurtherTraining3Entities4();

        // GET: api/Connects
        public IQueryable<Connect> GetConnect()
        {
            return db.Connect;
        }
        // GET: api/Connects/5
        [ResponseType(typeof(Connect))]
        public IHttpActionResult GetConnect(int id)
        {

            Connect connect = db.Connect.Find(id);
            if (connect == null)
            {
                return NotFound();
            }

            return Ok(connect);
        }
        [Route("~/api/GetLogin/{login?}")]
        public IHttpActionResult GetLoginPerson(string login)
        {
            var loginPerson = login.Split(' ');
            var loginMailToConnect = loginPerson[1];
            var loginPassword = loginPerson[0];
            var id = db.Connect.Select(x => new { x.IdConnect, x.PasswordC, x.MailToConnect }).FirstOrDefault(t => t.MailToConnect == loginMailToConnect &&  t.PasswordC == loginPassword);

            if (id == null)
            {
                return NotFound();
            }
            Connect connect = db.Connect.Find(id.IdConnect);
            return Ok(connect);
        }


        [Route("~/api/GetConnectByNameAndPassword")]
        [ResponseType(typeof(Connect))]
        public IHttpActionResult GetConnectByNameAndPassword(Connect connectLogin)
        {

            Connect connect = db.Connect.Find(connectLogin.IdConnect);
            if (connect == null)
            {
                return NotFound();
            }

            return Ok(connect);
        }
        // PUT: api/Connects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConnect(int id, Connect connect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != connect.IdConnect)
            {
                return BadRequest();
            }

            db.Entry(connect).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectExists(id))
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

        // POST: api/Connects
        [ResponseType(typeof(Connect))]
        public IHttpActionResult PostConnect(Connect connect)
        {
            var Connect = db.Connect.Select(r => new { r.PasswordC, r.MailToConnect }).Where(d => d.MailToConnect == connect.MailToConnect && d.PasswordC == connect.PasswordC).Count();
            if (!ModelState.IsValid || Connect>0)
            {
                return BadRequest(ModelState);
            }

            db.Connect.Add(connect);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = connect.IdConnect }, connect);
        }

        // DELETE: api/Connects/5
        [ResponseType(typeof(Connect))]
        public IHttpActionResult DeleteConnect(int id)
        {
            Connect connect = db.Connect.Find(id);
            if (connect == null)
            {
                return NotFound();
            }

            db.Connect.Remove(connect);
            db.SaveChanges();

            return Ok(connect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConnectExists(int id)
        {
            return db.Connect.Count(e => e.IdConnect == id) > 0;
        }
        [Route("~/api/sendEmails")]
        [ResponseType(typeof(Connect))]
        public Boolean GetsendEmail()
        {
            SendEmails();
            return false; 
            
        }
        //איפוס סיסמה
        public static void SendEmails()
        {
            var message = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            message.From = new MailAddress("sitefurthertraining@gmail.com");
            message.To.Add(new MailAddress("pnina15461@gmail.com"));
            message.Subject = "השלמת תהליך איפוס סיסמה";
            message.Body = "שלום   ";
            message.Body += "<br>";
            message.Body += "קוד האימות שלך 207226 ";
            message.Body += "<br>";
            message.Body += "Further Training ";
            message.Body += "  ";
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            var client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(message.From.User, "furtherTraining");
            client.Host = "smtp.gmail.com";
            client.Send(message);

            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //mail.From = new MailAddress("sitefurthertraining@gmail.com");
            //mail.To.Add("pnina15461@gmail.com");
            //mail.Subject = "Test Mail";
            //mail.Body = "This is for testing SMTP mail from GMAIL";

            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);
            //MessageBox.Show("mail Send");
        }
    }
}