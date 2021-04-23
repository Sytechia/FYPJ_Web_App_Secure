using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPJ_Web_App_Insecure.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;


namespace FYPJ_Web_App_Insecure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly databaseContext _db;
        public ProductController(databaseContext db)
        {
            _db = db;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProduct()
        {

            return await _db.Products.ToListAsync();
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var products = await _db.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        [HttpPut("{id}")]
        /*public async Task<IActionResult> PutProduct(int id, Products products)
        {
            if (id != products.ProductId)
            {
                return BadRequest();
            }

            _db.Entry(products).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            //var product1 = JsonConvert.DeserializeObject<Products>(products);
            _db.Products.Add(products);
            await _db.SaveChangesAsync();


            return CreatedAtAction("GetProduct", new { id = products.ProductId }, products);


        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Products>> DeleteProduct(int id)
        {
            var products = await _db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _db.Products.Remove(products);
            await _db.SaveChangesAsync();

            return products;
        }

       


        /*private bool ProductExists(int id)
        {
            return _db.Products.Any(e => e.ProductId == id);
        }*/



    }
}

