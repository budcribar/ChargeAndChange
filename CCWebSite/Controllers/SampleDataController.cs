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



        [HttpGet("[action]")]
        //public async Task< IEnumerable<EVSpecs>> EVSpecs()

         public IEnumerable<EVSpecs> EVSpecs()
        {
            //var repo = new DocumentDBRepository<EVSpecs>();

            //foreach (var item in Specs)
            //    repo.CreateItemAsync(item).Wait();


            //var collections = db.ListCollectionNames();

            ////db.CreateCollection("bev");



            //var col = db.GetCollection<string>("bev");

            //col.InsertOne

            //col.InsertMany(Specs, new InsertManyOptions { BypassDocumentValidation = true });

            var result = respository.GetItemsAsync(x => true).Result;

            // return await repo.GetItemsAsync(x => true);

            return result;

      
        }

        
    }
}
