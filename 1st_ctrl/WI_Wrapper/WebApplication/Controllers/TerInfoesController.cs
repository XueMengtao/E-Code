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
    public class TerInfoesController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/TerInfoes
        public IQueryable<TerInfo> GetTerInfoes()
        {
            return db.TerInfoes;
        }

        // GET: api/TerInfoes/5
        [ResponseType(typeof(TerInfo))]
        public IHttpActionResult GetTerInfo(int id)
        {
            TerInfo terInfo = db.TerInfoes.Find(id);
            if (terInfo == null)
            {
                return NotFound();
            }

            return Ok(terInfo);
        }

        // PUT: api/TerInfoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTerInfo(int id, TerInfo terInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != terInfo.TerInfoID)
            {
                return BadRequest();
            }

            db.Entry(terInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerInfoExists(id))
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

        // POST: api/TerInfoes
        [ResponseType(typeof(TerInfo))]
        public IHttpActionResult PostTerInfo(TerInfo terInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TerInfoes.Add(terInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = terInfo.TerInfoID }, terInfo);
        }

        // DELETE: api/TerInfoes/5
        [ResponseType(typeof(TerInfo))]
        public IHttpActionResult DeleteTerInfo(int id)
        {
            TerInfo terInfo = db.TerInfoes.Find(id);
            if (terInfo == null)
            {
                return NotFound();
            }

            db.TerInfoes.Remove(terInfo);
            db.SaveChanges();

            return Ok(terInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TerInfoExists(int id)
        {
            return db.TerInfoes.Count(e => e.TerInfoID == id) > 0;
        }
    }
}