using FYPJ_Web_App_Insecure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace FYPJ_Web_App_Insecure.Controllers
{
    public class InsecureController : Controller
    {
        private readonly databaseContext _context;

        public InsecureController( databaseContext context)
        {
            
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeed()
        {
            string content = string.Empty;
            using (Stream receiveStream = HttpContext.Request.Body)
            {
                using (StreamReader reader = new StreamReader(receiveStream))
                {
                    content = reader.ReadToEnd();
                }
            }

            var entry = JsonConvert.DeserializeObject<Customer>(content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto // A6 - Insecure Deserialization - You should instead use TypeNameHandling.None
            });

            _context.Add(entry);
            await _context.SaveChangesAsync();

            return Ok();


        }
    }
}
