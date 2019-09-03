using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CCWebSite.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IDocumentDBRepository<EVSpecs> respository;
        public SampleDataController(IDocumentDBRepository<EVSpecs> Respository)
        {
            this.respository = Respository;
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] EVSpecs item)
        {
            if (ModelState.IsValid)
            {
                await respository.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpDelete("EVSpecs/{id}")]
        public async void Delete(string id)
        {
            await respository.DeleteItemAsync(id);
        }

        [HttpPatch("EVSpecs/{id}")]
        public async void Patch(string id, [FromBody]EVSpecs bev)
        {
            bev.Id = id;
            await respository.UpdateItemAsync(id, bev);
        }

        [HttpPut("EVSpecs")]
        public async void Put([FromBody]EVSpecs bev)
        {
            bev.Id = Guid.NewGuid().ToString();
            await respository.CreateItemAsync(bev);
        }


        [HttpGet("[action]")]
        public  IEnumerable<EVSpecs> EVSpecs()
        {
            return respository.GetItemsAsync(x => true).Result;
        }

        
    }
}
