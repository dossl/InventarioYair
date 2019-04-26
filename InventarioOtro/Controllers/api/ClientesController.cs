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
using AutoMapper;
using InventarioOtro.Dtos;
using InventarioOtro.Models;


namespace InventarioOtro.Controllers.api
{
    public class ClientesController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();


        // GET: api/ApiCliente
        public IEnumerable<Cliente> GetClientes(string query)
        {
            var a = _db.Clientes
                .Where(c => (c.Cfirstname.Contains(query) || c.Clastname.Contains(query)) && c.Lactivo)
                .ToList();
            return a;
        }

        // GET: api/ApiCliente/5
        public Cliente GetClientes(int id)
        {
            var cliente = _db.Clientes.SingleOrDefault(c => c.ID == id);
            if (cliente == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return cliente;
        }

        // POST: api/ApiCliente
        [HttpPost]
        public Cliente CreateCliente([FromBody]Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            _db.Clientes.Add(cliente);
            _db.SaveChanges();
            return cliente;
        }

        // PUT: api/ApiCliente/5
        public void UpdateCliente(int id, [FromBody]Cliente cliente)
        {

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var clienteInDb = _db.Clientes.SingleOrDefault(c => c.ID == id);

            if (clienteInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            clienteInDb.Ccidentidad = cliente.Ccidentidad;
            clienteInDb.Cdireccion = cliente.Cdireccion;
            clienteInDb.Cfirstname = cliente.Cfirstname;
            clienteInDb.Clastname = cliente.Clastname;
            clienteInDb.Cnotas = cliente.Cnotas;
            clienteInDb.Cnumtelefono = cliente.Cnumtelefono;
            clienteInDb.Lactivo = cliente.Lactivo;

            _db.SaveChanges();



        }

        // DELETE: api/ApiCliente/5
        [HttpDelete]
        public void DeleteCliente(int id)
        {
            var cliente = _db.Clientes.SingleOrDefault(c => c.ID == id);
            if (cliente == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _db.Clientes.Remove(cliente);
            _db.SaveChanges();

        }
    }
}

  