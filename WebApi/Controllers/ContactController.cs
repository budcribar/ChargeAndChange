using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using TeslaSuperchargers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace CCWebSite.Controllers
{
    // https://docs.microsoft.com/en-us/azure/azure-functions/functions-openapi-definition
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IDocumentDBRepository<Contact> repository;
        public ContactController(IDocumentDBRepository<Contact> Respository)
        {
            this.repository = Respository;
        }

        [HttpGet("GetContact/{email}")]
        [FunctionName(nameof(GetContact))]
        public async Task<Contact> GetContact([HttpTrigger("get", Route = "Contact/GetContact/{email}")] HttpRequest request, string email)    
        {
            return await repository.GetItemAsync(x => x.Email.ToLower() == email.ToLower());
        }

        [HttpGet("GetContactById/{id}")]
        public async Task<Contact> GetContactById([HttpTrigger("get", Route = "Contact/GetContactById/{id}")] HttpRequest request, string id)
        {
            return await repository.GetItemAsync(x => x.Id == id);
        }

        [HttpDelete("DeleteContact/{id}")]
        [FunctionName(nameof(DeleteContact))]
        public async Task<IActionResult> DeleteContact([HttpTrigger("delete", Route = "Contact/DeleteContact/{id}")] HttpRequest request, string id)
        {
            // Todo verify id
            await repository.DeleteItemAsync(id);
            return Ok();
        }

        [HttpPatch("PatchContact/{id}")]
        [FunctionName(nameof(PatchContact))]
       
        public async Task<IActionResult> PatchContact([HttpTrigger("patch", Route = "Contact/PatchContact/{id}")] HttpRequest request, string id)

        {
            var body = await request.ReadAsStringAsync();
            Contact contact = JsonConvert.DeserializeObject<Contact>(body);

            if (contact == null) return NoContent(); ;
            contact.Id = id;

            // Don't overwrite hashed password
            var old = await repository.GetItemAsync(x => x.Id == contact.Id);
            contact.HashedPassword = old.HashedPassword;

            await repository.UpdateItemAsync(id, contact);
            return Ok();
        }

        [HttpPut("PutContact")]
        [FunctionName(nameof(PutContact))]
        public async Task<IActionResult> PutContact([HttpTrigger("put", Route = "Contact/PutContact")] HttpRequest request)
        {
            var body = await request.ReadAsStringAsync();
            Contact contact = JsonConvert.DeserializeObject<Contact>(body);

            if (contact == null) return NoContent(); 
            contact.Id = Guid.NewGuid().ToString();
            if (contact.Password.Length > 0)
            {
                var h = new PasswordHasher<Contact>();
                contact.HashedPassword = h.HashPassword(contact, contact.Password);
                contact.Password = "";
                contact.DateUpdated = DateTime.Now;
            }
           
            await repository.CreateItemAsync(contact);
            return Ok();
        }

        [FunctionName(nameof(Login))]
        public async Task<Contact?> Login([HttpTrigger("post", Route = "Contact/login")] HttpRequest request)
        {
            var body = await request.ReadAsStringAsync();
            Contact contact = JsonConvert.DeserializeObject<Contact>(body);

            if (contact == null) return null;

            Contact c = await repository.GetItemAsync(x => x.Email.ToLower() == contact.Email.ToLower());

            if (c == null) return null;

            var h = new PasswordHasher<Contact>();
            var pvr = h.VerifyHashedPassword(c, c.HashedPassword, contact.Password);

            var loggedin = pvr == PasswordVerificationResult.Success;

            if (!loggedin) return null;

            return c;
        }
       
        [HttpGet("[action]")]
        [FunctionName(nameof(Contacts))]
        public async Task<IEnumerable<Contact>> Contacts([HttpTrigger("get", Route = "Contact/contacts")] HttpRequest request)
        {
            return await repository.GetItemsAsync(x => true);
        }

        [FunctionName(nameof(ContactsInSubdivision))]
        public async Task<IEnumerable<Contact>> ContactsInSubdivision([HttpTrigger("get", Route = "Contact/ContactsInSubdivision/{subdivision}")] HttpRequest request, string subdivision)
        {
            var result = await Download.DownloadJObjectAsync(@"https://apps.larimer.org/api/assessor/property/?prop=property&parcel=undefined&scheduleNumber=undefined&serialIdentification=undefined&name=&fromAddrNum=undefined&toAddrNum=undefined&address=&city=FORT%20COLLINS&subdivisionNumber=undefined&sales=any&subdivisionName=WILLOW%20SPRINGS%20PUD");
            var records = result["records"];

            var results = records.Select(x => JsonConvert.DeserializeObject<LarimerCountyRecord>(x.ToString()).ToContact(subdivision)).Where(x => x != null).ToList();

            repository.CreateItemsAsync(results.ToArray());

            return results;
        }
    }
}
