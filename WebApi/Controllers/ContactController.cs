using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using TeslaSuperchargers;
using Microsoft.AspNetCore.Identity;

namespace CCWebSite.Controllers
{
    // https://docs.microsoft.com/en-us/azure/azure-functions/functions-openapi-definition
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IDocumentDBRepository<Contact> repository;
        public ContactController(IDocumentDBRepository<Contact> Respository)
        {
            this.repository = Respository;
        }

        //[HttpPost]
        //[ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] Contact item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await respository.UpdateItemAsync(item.Id, item);
        //        return RedirectToAction("Index");
        //    }

        //    return View(item);
        //}

        [HttpGet("GetContact/{email}")]
        public  Task<Contact> Get(string email)
        {
            return repository.GetItemAsync(x => x.Email.ToLower() == email.ToLower());
        }

        [HttpGet("GetContactById/{id}")]
        public Task<Contact> GetContactById(string id)
        {
            return repository.GetItemAsync(x => x.Id == id);
        }

        [HttpDelete("DeleteContact/{id}")]
        public async void Delete(string id)
        {
            await repository.DeleteItemAsync(id);
        }

        [HttpPatch("PatchContact/{id}")]
        public async void Patch(string id, [FromBody]Contact bev)
        {
            if (bev == null) return;
            bev.Id = id;

            // Don't overwrite hashed password
            var old = await repository.GetItemAsync(x => x.Id == bev.Id);
            bev.HashedPassword = old.HashedPassword;

            await repository.UpdateItemAsync(id, bev);
        }

        [HttpPut("PutContact")]
        public async void Put([FromBody]Contact contact)
        {
            if (contact == null) return;
            contact.Id = Guid.NewGuid().ToString();
            if (contact.Password.Length > 0)
            {
                var h = new PasswordHasher<Contact>();
                contact.HashedPassword = h.HashPassword(contact, contact.Password);
                contact.Password = "";
                contact.DateUpdated = DateTime.Now;
            }
           
            await repository.CreateItemAsync(contact);
        }

        [HttpPost("login")]
        public async Task<Contact> Login([FromBody]Contact bev)
        {
            if (bev == null) return null;

            Contact c = await repository.GetItemAsync(x => x.Email.ToLower() == bev.Email.ToLower());

            if (c == null) return null;

            var h = new PasswordHasher<Contact>();
            var pvr = h.VerifyHashedPassword(c, c.HashedPassword, bev.Password);

            var loggedin = pvr == PasswordVerificationResult.Success;

            if (!loggedin) return null;

            return c;
        }

        [HttpGet("[action]")]
        public  IEnumerable<Contact> Contacts()
        {
            return repository.GetItemsAsync(x => true).Result;
        }

        [HttpGet("ContactsInSubdivision/{subdivision}")]
        public IEnumerable<Contact> ContactsFrom(string subdivision)
        {
            var result = Download.DownloadJObject(@"https://apps.larimer.org/api/assessor/property/?prop=property&parcel=undefined&scheduleNumber=undefined&serialIdentification=undefined&name=&fromAddrNum=undefined&toAddrNum=undefined&address=&city=FORT%20COLLINS&subdivisionNumber=undefined&sales=any&subdivisionName=WILLOW%20SPRINGS%20PUD");
            var records = result["records"];

            //foreach (var r in records)
            //{
            //    var s = r.ToString();
            //    var lcr = JsonConvert.DeserializeObject<LarimerCountyRecord>(s);
            //    var c = lcr.ToContact(subdivision);
            //}

            var results = records.Select(x => JsonConvert.DeserializeObject<LarimerCountyRecord>(x.ToString()).ToContact(subdivision)).Where(x => x != null).ToList();

            repository.CreateItemsAsync(results.ToArray());

            return results;
        }
    }
}
