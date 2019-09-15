using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CCWebSite.Controllers
{
    public class ChartData
    {
        public string Name { get; set; }
        public double? Y { get; set; }
    }

    [Route("api/[controller]")]
    public class BEVController : Controller
    {
        private readonly IDocumentDBRepository<EVSpecs> respository;
        public BEVController(IDocumentDBRepository<EVSpecs> Respository)
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
            if (bev == null) return;
            bev.Id = id;
            await respository.UpdateItemAsync(id, bev);
        }

        [HttpPut("EVSpecs")]
        public async void Put([FromBody]EVSpecs bev)
        {
            if (bev == null) return;
            bev.Id = Guid.NewGuid().ToString();
            await respository.CreateItemAsync(bev);
        }

        private double? GetValue(EVSpecs evspec, string spec)
        {
            var o =  typeof(EVSpecs).GetProperty(spec).GetValue(evspec);
            if (o == null) return null;

            return ((IConvertible)o).ToDouble(null);
        }

        [HttpGet("Spec/{spec}/{availableOnly}")]
        public IEnumerable<ChartData> Spec(string spec, Boolean availableOnly)
        {
            return respository.GetItemsAsync(x => true).Result.Where(y => !availableOnly || y.Available).Select(x => new ChartData { Name = x.Manufacturer + ' ' + x.Model, Y = GetValue(x, spec) }).Where(x => x.Y != null).OrderBy(x => x.Y);
        }

        [HttpGet("[action]")]
        public  IEnumerable<EVSpecs> EVSpecs()
        {
            return respository.GetItemsAsync(x => true).Result;
        }

        
    }
}
