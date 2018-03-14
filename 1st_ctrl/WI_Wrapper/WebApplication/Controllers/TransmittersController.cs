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
    public class TransmittersController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/Transmitters
        public IQueryable<Transmitter> GetTransmitters()
        {
            return db.Transmitters;
        }

        // GET: api/Transmitters/5
        [ResponseType(typeof(Transmitter))]
        public IHttpActionResult GetTransmitter(int id)
        {
            Transmitter transmitter = db.Transmitters.Find(id);
            if (transmitter == null)
            {
                return NotFound();
            }

            return Ok(transmitter);
        }

        // PUT: api/Transmitters/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransmitter(int id, Transmitter transmitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transmitter.TransmitterID)
            {
                return BadRequest();
            }

            db.Entry(transmitter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransmitterExists(id))
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

        // POST: api/Transmitters
        [ResponseType(typeof(Transmitter))]
        public IHttpActionResult PostTransmitter(Transmitter transmitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transmitters.Add(transmitter);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transmitter.TransmitterID }, transmitter);
        }

        // DELETE: api/Transmitters/5
        [ResponseType(typeof(Transmitter))]
        public IHttpActionResult DeleteTransmitter(int id)
        {
            Transmitter transmitter = db.Transmitters.Find(id);
            if (transmitter == null)
            {
                return NotFound();
            }

            db.Transmitters.Remove(transmitter);
            db.SaveChanges();

            return Ok(transmitter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransmitterExists(int id)
        {
            return db.Transmitters.Count(e => e.TransmitterID == id) > 0;
        }
    }
}