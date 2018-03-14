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
    public class TaskFileRelationsController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/TaskFileRelations
        public IQueryable<TaskFileRelation> GetTaskFileRelations()
        {
            return db.TaskFileRelations;
        }

        // GET: api/TaskFileRelations/5
        [ResponseType(typeof(TaskFileRelation))]
        public IHttpActionResult GetTaskFileRelation(int id)
        {
            TaskFileRelation taskFileRelation = db.TaskFileRelations.Find(id);
            if (taskFileRelation == null)
            {
                return NotFound();
            }

            return Ok(taskFileRelation);
        }

        // PUT: api/TaskFileRelations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskFileRelation(int id, TaskFileRelation taskFileRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskFileRelation.TaskFileRelationID)
            {
                return BadRequest();
            }

            db.Entry(taskFileRelation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskFileRelationExists(id))
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

        // POST: api/TaskFileRelations
        [ResponseType(typeof(TaskFileRelation))]
        public IHttpActionResult PostTaskFileRelation(TaskFileRelation taskFileRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskFileRelations.Add(taskFileRelation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = taskFileRelation.TaskFileRelationID }, taskFileRelation);
        }

        // DELETE: api/TaskFileRelations/5
        [ResponseType(typeof(TaskFileRelation))]
        public IHttpActionResult DeleteTaskFileRelation(int id)
        {
            TaskFileRelation taskFileRelation = db.TaskFileRelations.Find(id);
            if (taskFileRelation == null)
            {
                return NotFound();
            }

            db.TaskFileRelations.Remove(taskFileRelation);
            db.SaveChanges();

            return Ok(taskFileRelation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskFileRelationExists(int id)
        {
            return db.TaskFileRelations.Count(e => e.TaskFileRelationID == id) > 0;
        }
    }
}