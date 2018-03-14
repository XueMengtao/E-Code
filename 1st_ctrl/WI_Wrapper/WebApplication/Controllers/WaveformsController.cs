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
    public class WaveformsController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/Waveforms
        public IQueryable<Waveform> GetWaveforms()
        {
            return db.Waveforms;
        }

        // GET: api/Waveforms/5
        [ResponseType(typeof(Waveform))]
        public IHttpActionResult GetWaveform(int id)
        {
            Waveform waveform = db.Waveforms.Find(id);
            if (waveform == null)
            {
                return NotFound();
            }

            return Ok(waveform);
        }

        // PUT: api/Waveforms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWaveform(int id, Waveform waveform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != waveform.WaveformID)
            {
                return BadRequest();
            }

            db.Entry(waveform).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaveformExists(id))
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

        // POST: api/Waveforms
        [ResponseType(typeof(Waveform))]
        public IHttpActionResult PostWaveform(Waveform waveform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Waveforms.Add(waveform);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = waveform.WaveformID }, waveform);
        }

        // DELETE: api/Waveforms/5
        [ResponseType(typeof(Waveform))]
        public IHttpActionResult DeleteWaveform(int id)
        {
            Waveform waveform = db.Waveforms.Find(id);
            if (waveform == null)
            {
                return NotFound();
            }

            db.Waveforms.Remove(waveform);
            db.SaveChanges();

            return Ok(waveform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WaveformExists(int id)
        {
            return db.Waveforms.Count(e => e.WaveformID == id) > 0;
        }
    }
}