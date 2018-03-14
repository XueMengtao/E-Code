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
    public class AntennaeController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/Antennae
        public IQueryable<Antenna> GetAntennae()
        {
            return db.Antennae;
        }

        // GET: api/Antennae/5
        [ResponseType(typeof(Antenna))]
        public IHttpActionResult GetAntenna(int id)
        {
            Antenna antenna = db.Antennae.Find(id);
            if (antenna == null)
            {
                return NotFound();
            }

            return Ok(antenna);
        }

        // PUT: api/Antennae/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAntenna(int id, Antenna antenna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != antenna.AntennaID)
            {
                return BadRequest();
            }

            db.Entry(antenna).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AntennaExists(id))
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

        // POST: api/Antennae
        [ResponseType(typeof(Antenna))]
        public IHttpActionResult PostAntenna(Antenna antenna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Antennae.Add(antenna);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = antenna.AntennaID }, antenna);
        }

        // DELETE: api/Antennae/5
        [ResponseType(typeof(Antenna))]
        public IHttpActionResult DeleteAntenna(int id)
        {
            Antenna antenna = db.Antennae.Find(id);
            if (antenna == null)
            {
                return NotFound();
            }

            db.Antennae.Remove(antenna);
            db.SaveChanges();

            return Ok(antenna);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AntennaExists(int id)
        {
            return db.Antennae.Count(e => e.AntennaID == id) > 0;
        }
    }
}