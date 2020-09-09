using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using TeslaSuperchargers;
//using TeslaSuperchargers;
//using Microsoft.AspNetCore.Identity;

namespace CCWebSite.Controllers
{
    //[Route("api/[controller]")]
    public class ContactController 
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

        //[HttpGet("Contact/{email}")]

        public Task<Contact?> GetById(string id)
        {
            return repository.GetItemAsync(x => x.Id == id);
        }

        public  Task<Contact?> Get(string email)
        {
            return repository.GetItemAsync(x => x.Email.ToLower() == email.ToLower());
        }

        //[HttpDelete("Contacts/{id}")]
        public Task Delete(string id)
        {
            return repository.DeleteItemAsync(id);
        }

        //[HttpPatch("Contacts/{id}")]
        public Task Patch(string id, Contact contact)
        {
            //if (bev == null) return;
            contact.Id = id;

            // Don't overwrite hashed password
            //var old = repository.GetItemAsync(x => x.Id == contact.Id).Result;
            //contact.HashedPassword = old.HashedPassword;

            return repository.UpdateItemAsync(id, contact);
        }

        //[HttpPut("Contacts")]
        public Task Put(Contact contact)
        {
            //if (contact == null) return null;
            contact.Id = Guid.NewGuid().ToString();
            if (contact.Password.Length > 0)
            {
                var h = new PasswordHasher<Contact>();
                contact.HashedPassword = h.HashPassword(contact, contact.Password);
                contact.Password = "";
                contact.DateUpdated = DateTime.Now;
            }
           
            return repository.CreateItemAsync(contact);
        }

        //[HttpPost("login")]
        public async Task<Contact?> Login(Contact bev)
        {
            if (bev == null) return null;

            Contact? c = await repository.GetItemAsync(x => x.Email.ToLower() == bev.Email.ToLower());

            if (c == null) return null;

            var h = new PasswordHasher<Contact>();
            var pvr = h.VerifyHashedPassword(c, c.HashedPassword, bev.Password);

            var loggedin = pvr == PasswordVerificationResult.Success;

            if (!loggedin) return null;

            return c;
        }

        
        public  Task<IEnumerable<Contact>> Contacts()
        {
            return repository.GetItemsAsync(x => true);
        }

       
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

            List<Task<Microsoft.Azure.Documents.Document>> tasks = new List<Task<Microsoft.Azure.Documents.Document>>();
            foreach (var r in results)
            {
                var t = repository.CreateItemAsync(r);
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            //repository.CreateItemsAsync(results.ToArray());

            return results;
        }
    }
}
