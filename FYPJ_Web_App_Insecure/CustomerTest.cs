using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using FYPJ_Web_App_Insecure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FYPJ_Web_App_Insecure
{
    public class CustomerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var jsonString = File.ReadAllText("Customer.json");
            var jsonModel = JsonSerializer.Deserialize<Customer>(jsonString, options);
            var customerJson = JsonSerializer.Serialize(jsonModel, options);
            Console.WriteLine(customerJson);
        }
        
    }
}
