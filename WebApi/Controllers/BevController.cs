using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace CCWebSite.Controllers
{
    public class ChartData
    {
        public string Name { get; set; }
        public double? Y { get; set; }
    }

    [Route("api/[controller]")]
    //WEBAPI public class BEVController : Controller
    public class BEVController : ControllerBase
    {
        private readonly IDocumentDBRepository<EVSpecs> respository;
        public BEVController(IDocumentDBRepository<EVSpecs> Respository)
        {
            this.respository = Respository;
        }

        [HttpDelete("DeleteEVSpecs/{id}")]
        [FunctionName(nameof(DeleteEVSpecs))]
        ////WEBAPI public async void DeleteEVSpecs(string id)
        public async Task<IActionResult> DeleteEVSpecs([HttpTrigger("delete", Route = "DeleteEVSpecs/{id}")] HttpRequest request, string id)
        {
            // TODO cleanup
            await respository.DeleteItemAsync(id);
            return Ok();
        }

        [HttpPatch("PatchEVSpecs/{id}")]
        [FunctionName(nameof(PatchEVSpecs))]
        //public async void PatchEVSpecs(string id, [FromBody]EVSpecs bev)
        public async Task<IActionResult> PatchEVSpecs([HttpTrigger("patch", Route = "PatchEVSpecs/{id}")] HttpRequest request, string id)
        {
            var body = await request.ReadAsStringAsync();
            EVSpecs bev = JsonConvert.DeserializeObject<EVSpecs>(body);
            if (bev == null) return NoContent();
            bev.Id = id;
            await respository.UpdateItemAsync(id, bev);
            return Ok();
        }

        [HttpPost("PostEVSpecs")]
        [FunctionName(nameof(PostEVSpecs))]
        //public async void PostEVSpecs([FromBody]EVSpecs bev)
        public async Task<IActionResult> PostEVSpecs([HttpTrigger("post", Route = "PostEVSpecs")] HttpRequest request)

        {
            var body = await request.ReadAsStringAsync();
            EVSpecs bev = JsonConvert.DeserializeObject<EVSpecs>(body);          
            if (bev == null) return NoContent();
            bev.Id = Guid.NewGuid().ToString();
            await respository.CreateItemAsync(bev);
            return Ok();
        }

        private double? GetValue(EVSpecs evspec, string spec)
        {
            var o =  typeof(EVSpecs).GetProperty(spec).GetValue(evspec);
            if (o == null) return null;

            return ((IConvertible)o).ToDouble(null);
        }

       
     
        [HttpGet("GetSpecs/{spec}/{availableOnly}")]
        [FunctionName(nameof(GetSpecs))]
        public async Task<List<ChartData>> GetSpecs([HttpTrigger("get", Route = "GetSpecs/{spec}/{availableOnly}")] HttpRequest request, string spec, bool availableOnly)
        {
            return (await respository.GetItemsAsync(x => true)).Where(y => !availableOnly || y.Available).Select(x => new ChartData { Name = x.Manufacturer + ' ' + x.Model, Y = GetValue(x, spec) }).Where(x => x.Y != null).OrderBy(x => x.Y).ToList();
        }

        [HttpGet("[action]")]
        [FunctionName(nameof(EVSpecs))]
        public async Task<IEnumerable<EVSpecs>> EVSpecs([HttpTrigger("get", Route = "EVSpecs")] HttpRequest request)
        {
            return await respository.GetItemsAsync(x => true);
        }

        
    }
}
