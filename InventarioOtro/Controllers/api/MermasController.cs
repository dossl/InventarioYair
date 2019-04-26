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
    public class MermasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Mermas
        public List<Producto> GetMermas(string query)
        {
            List<Merma> list= db.Mermas.Include(p => p.Producto).Where(m => (m.Producto.Lactivo && m.Cantidad > 0 && m.Producto.Carticulo.Contains(query))).ToList();
            List < Producto > res = new List<Producto>();
            foreach (var merma in list)
            {
                Producto p = new Producto();
                p = merma.Producto;
                p.Ndisponibilidad = merma.Cantidad;
                p.Nprecmayoris = merma.Precio;
                p.Nprecminoris = merma.Precio;
                p.Npreccosto = 0;
                res.Add(p);
            }
            return res;
        }

        // GET: api/Mermas/5
        [ResponseType(typeof(Merma))]
        public async Task<IHttpActionResult> GetMerma(int id)
        {
            Merma merma = await db.Mermas.FindAsync(id);
            if (merma == null)
            {
                return NotFound();
            }

            return Ok(merma);
        }

        // PUT: api/Mermas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMerma(int id, Merma merma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != merma.Id)
            {
                return BadRequest();
            }

            db.Entry(merma).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MermaExists(id))
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

        // POST: api/Mermas
        [ResponseType(typeof(Merma))]
        public async Task<IHttpActionResult> PostMerma(Merma merma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Mermas.Add(merma);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = merma.Id }, merma);
        }

        // DELETE: api/Mermas/5
        [ResponseType(typeof(Merma))]
        public async Task<IHttpActionResult> DeleteMerma(int id)
        {
            Merma merma = await db.Mermas.FindAsync(id);
            if (merma == null)
            {
                return NotFound();
            }

            db.Mermas.Remove(merma);
            await db.SaveChangesAsync();

            return Ok(merma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MermaExists(int id)
        {
            return db.Mermas.Count(e => e.Id == id) > 0;
        }
    }
}