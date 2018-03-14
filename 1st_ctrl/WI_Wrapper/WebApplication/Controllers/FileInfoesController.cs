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
    public class FileInfoesController : ApiController
    {
        private EMCContext db = new EMCContext();

        // GET: api/FileInfoes
        public IQueryable<FileInfo> GetFileInfoes()
        {
            return db.FileInfoes;
        }

        // GET: api/FileInfoes/5
        [ResponseType(typeof(FileInfo))]
        public IHttpActionResult GetFileInfo(int id)
        {
            FileInfo fileInfo = db.FileInfoes.Find(id);
            if (fileInfo == null)
            {
                return NotFound();
            }

            return Ok(fileInfo);
        }

        // PUT: api/FileInfoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFileInfo(int id, FileInfo fileInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fileInfo.FileInfoID)
            {
                return BadRequest();
            }

            db.Entry(fileInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileInfoExists(id))
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

        // POST: api/FileInfoes
        [ResponseType(typeof(FileInfo))]
        public IHttpActionResult PostFileInfo(FileInfo fileInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FileInfoes.Add(fileInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fileInfo.FileInfoID }, fileInfo);
        }

        // DELETE: api/FileInfoes/5
        [ResponseType(typeof(FileInfo))]
        public IHttpActionResult DeleteFileInfo(int id)
        {
            FileInfo fileInfo = db.FileInfoes.Find(id);
            if (fileInfo == null)
            {
                return NotFound();
            }

            db.FileInfoes.Remove(fileInfo);
            db.SaveChanges();

            return Ok(fileInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FileInfoExists(int id)
        {
            return db.FileInfoes.Count(e => e.FileInfoID == id) > 0;
        }
    }
}