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
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IDocumentDBRepository<Contact> respository;
        public ContactController(IDocumentDBRepository<Contact> Respository)
        {
            this.respository = Respository;
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] Contact item)
        {
            if (ModelState.IsValid)
            {
                await respository.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpGet("Contact/{email}")]
        public  Task<Contact> Get(string email)
        {
            return respository.GetItemAsync(x => x.Email.ToLower() == email.ToLower());
        }

        [HttpDelete("Contacts/{id}")]
        public async void Delete(string id)
        {
            await respository.DeleteItemAsync(id);
        }

        [HttpPatch("Contacts/{id}")]
        public async void Patch(string id, [FromBody]Contact bev)
        {
            if (bev == null) return;
            bev.Id = id;

            // Don't overwrite hashed password
            var old = await respository.GetItemAsync(x => x.Id == bev.Id);
            bev.HashedPassword = old.HashedPassword;

            await respository.UpdateItemAsync(id, bev);
        }

        [HttpPut("Contacts")]
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
           
            await respository.CreateItemAsync(contact);
        }

        [HttpPost("login")]
        public async Task<Contact> Login([FromBody]Contact bev)
        {
            if (bev == null) return null;

            Contact c = await respository.GetItemAsync(x => x.Email.ToLower() == bev.Email.ToLower());

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
            return respository.GetItemsAsync(x => true).Result;
        }

        [HttpGet("ContactsFrom/{subdivision}")]
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

            respository.CreateItemsAsync(results.ToArray());

            return results;
        }
    }
}
