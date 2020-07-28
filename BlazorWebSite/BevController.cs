using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCWebSite.Controllers
{
    public class ChartData
    {
        public string Name { get; set; }
        public double? Y { get; set; }
    }

    
    public class BEVController 
    {
        private readonly IDocumentDBRepository<EVSpecs> respository;
        public BEVController(IDocumentDBRepository<EVSpecs> Respository)
        {
            this.respository = Respository;
        }

        //[HttpPost]
        //[ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] EVSpecs item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await respository.UpdateItemAsync(item.Id, item);
        //        return RedirectToAction("Index");
        //    }

        //    return View(item);
        //}

       
        public Task Delete(string id)
        {
            return respository.DeleteItemAsync(id);
        }

        
        public Task Patch(string id,EVSpecs bev)
        {
            if (bev == null) return Task.CompletedTask;
            bev.Id = id;
            return respository.UpdateItemAsync(id, bev);
        }

        
        public Task Post(EVSpecs bev)
        {
            if (bev == null) return Task.CompletedTask; 
            bev.Id = Guid.NewGuid().ToString();
            return respository.CreateItemAsync(bev);
        }

        private double? GetValue(EVSpecs evspec, string spec)
        {
            var o =  typeof(EVSpecs).GetProperty(spec).GetValue(evspec);
            if (o == null) return null;

            return ((IConvertible)o).ToDouble(null);
        }

        
        public IEnumerable<ChartData> Spec(string spec, Boolean availableOnly)
        {
            return respository.GetItemsAsync(x => true).Result.Where(y => !availableOnly || y.Available).Select(x => new ChartData { Name = x.Manufacturer + ' ' + x.Model, Y = GetValue(x, spec) }).Where(x => x.Y != null).OrderBy(x => x.Y);
        }

       
        public Task<IEnumerable<EVSpecs>> EVSpecs()
        {
            return respository.GetItemsAsync(x => true);
        }

        
    }
}
