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
    public class TaskInfoesController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/TaskInfoes
        public IQueryable<TaskInfo> GetTaskInfoes()
        {
            return db.TaskInfoes;
        }

        // GET: api/TaskInfoes/5
        [ResponseType(typeof(TaskInfo))]
        public IHttpActionResult GetTaskInfo(int id)
        {
            TaskInfo taskInfo = db.TaskInfoes.Find(id);
            if (taskInfo == null)
            {
                return NotFound();
            }

            return Ok(taskInfo);
        }

        // PUT: api/TaskInfoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskInfo(int id, TaskInfo taskInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskInfo.TaskInfoID)
            {
                return BadRequest();
            }

            db.Entry(taskInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskInfoExists(id))
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

        // POST: api/TaskInfoes
        [ResponseType(typeof(TaskInfo))]
        public IHttpActionResult PostTaskInfo(TaskInfo taskInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskInfoes.Add(taskInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = taskInfo.TaskInfoID }, taskInfo);
        }

        // DELETE: api/TaskInfoes/5
        [ResponseType(typeof(TaskInfo))]
        public IHttpActionResult DeleteTaskInfo(int id)
        {
            TaskInfo taskInfo = db.TaskInfoes.Find(id);
            if (taskInfo == null)
            {
                return NotFound();
            }

            db.TaskInfoes.Remove(taskInfo);
            db.SaveChanges();

            return Ok(taskInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskInfoExists(int id)
        {
            return db.TaskInfoes.Count(e => e.TaskInfoID == id) > 0;
        }
    }
}