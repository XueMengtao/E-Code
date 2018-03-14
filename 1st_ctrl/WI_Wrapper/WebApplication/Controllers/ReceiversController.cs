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
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReceiversController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/Receivers
        public IQueryable<Receiver> GetReceivers()
        {
            return db.Receivers;
        }

        // GET: api/Receivers/5
        [ResponseType(typeof(Receiver))]
        public IHttpActionResult GetReceiver(int id)
        {
            Receiver receiver = db.Receivers.Find(id);
            if (receiver == null)
            {
                return NotFound();
            }

            return Ok(receiver);
        }

        // PUT: api/Receivers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReceiver(int id, Receiver receiver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != receiver.ReceiverID)
            {
                return BadRequest();
            }

            db.Entry(receiver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiverExists(id))
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

        // POST: api/Receivers
        [ResponseType(typeof(Receiver))]
        public IHttpActionResult PostReceiver(Receiver receiver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Receivers.Add(receiver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = receiver.ReceiverID }, receiver);
        }

        // DELETE: api/Receivers/5
        [ResponseType(typeof(Receiver))]
        public IHttpActionResult DeleteReceiver(int id)
        {
            Receiver receiver = db.Receivers.Find(id);
            if (receiver == null)
            {
                return NotFound();
            }

            db.Receivers.Remove(receiver);
            db.SaveChanges();

            return Ok(receiver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReceiverExists(int id)
        {
            return db.Receivers.Count(e => e.ReceiverID == id) > 0;
        }
    }
}