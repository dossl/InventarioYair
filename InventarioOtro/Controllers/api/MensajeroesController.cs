using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using InventarioOtro.Models;


namespace InventarioOtro.Controllers.api
{
    public class MensajeroesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       

        // GET: api/Mensajeroes
        public IEnumerable<Mensajero> GetMensajeros(string query)
        {
            return db.Mensajeros.Where(c => c.Nombre.Contains(query)).ToList();
        }

        // GET: api/Mensajeroes/5
        [ResponseType(typeof(Mensajero))]
        public async Task<IHttpActionResult> GetMensajero(int id)
        {
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            if (mensajero == null)
            {
                return NotFound();
            }

            return Ok(mensajero);
        }

        // PUT: api/Mensajeroes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMensajero(int id, Mensajero mensajero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mensajero.ID)
            {
                return BadRequest();
            }

            db.Entry(mensajero).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeroExists(id))
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

        // POST: api/Mensajeroes
        [ResponseType(typeof(Mensajero))]
        public async Task<IHttpActionResult> PostMensajero(Mensajero mensajero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Mensajeros.Add(mensajero);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mensajero.ID }, mensajero);
        }

        // DELETE: api/Mensajeroes/5
        [ResponseType(typeof(Mensajero))]
        public async Task<IHttpActionResult> DeleteMensajero(int id)
        {
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            if (mensajero == null)
            {
                return NotFound();
            }

            db.Mensajeros.Remove(mensajero);
            await db.SaveChangesAsync();

            return Ok(mensajero);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MensajeroExists(int id)
        {
            return db.Mensajeros.Count(e => e.ID == id) > 0;
        }

        
    }
}