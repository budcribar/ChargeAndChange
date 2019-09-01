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
        // ADD THIS PART TO YOUR CODE

        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://charge-and-change.documents.azure.com:443/";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "hKi3EPRIufbRmIFk5ehkAsp6OXyfUdLvEoHRhrv0ADVxIJyoPS3RA3JgqWRBhAYLTjoXkL9aZAOfvmMNl64SDw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "chargeAndChange";
        private string containerId = "bev";

        private static List<EVSpecs> Specs = new List<EVSpecs>
        {
            new EVSpecs {Id=Guid.NewGuid().ToString(), DateUpdated=new DateTime(2019,8,30), ModelYear=2019, Manufacturer="Audi", Model = "E-Tron", BodyStyle=null, Price=74800,FederalTaxCredit=null, Drive=null, CombinedRange=204, CityRange=null, HiwayRange=null, MotorPowerKw=265, Torque=414, BatteryCapacity=95, ChargingConnector=null, Weight=null, ZeroTo60=5.5, ZeroTo62=null, MaxChargePower=150, MinutesTo80PercentCharge=30, Notes="" },
            new EVSpecs {Id=Guid.NewGuid().ToString(),DateUpdated=new DateTime(2019,8,31), ModelYear=2019, Manufacturer="BMW", Model = "i3", BodyStyle=null, Price=44450,FederalTaxCredit=null, Drive=null, CombinedRange=153, CityRange=null, HiwayRange=null, MotorPowerKw=(int)Math.Round(170*.7457), Torque=null, BatteryCapacity=null, ChargingConnector=ChargingConnector.CCS, Weight=null, ZeroTo60=7.2, ZeroTo62=null, MaxChargePower=50, MinutesTo80PercentCharge=40, Notes="" },
            new EVSpecs {Id=Guid.NewGuid().ToString(),DateUpdated=new DateTime(2019,8,31), ModelYear=2019, Manufacturer="BMW", Model = "MINI Cooper", BodyStyle=null, Price=null,FederalTaxCredit=null, Drive=null, CombinedRange=null, CityRange=null, HiwayRange=null, MotorPowerKw=null, Torque=null, BatteryCapacity=null, ChargingConnector=null, Weight=null, ZeroTo60=null, ZeroTo62=null, MaxChargePower=null, MinutesTo80PercentCharge=null, Notes="" },

        };

        [HttpGet("[action]")]
        //public async Task< IEnumerable<EVSpecs>> EVSpecs()

         public IEnumerable<EVSpecs> EVSpecs()
        {
            var repo = new DocumentDBRepository<EVSpecs>();

            //foreach (var item in Specs)
            //    repo.CreateItemAsync(item).Wait();


            //var collections = db.ListCollectionNames();

            ////db.CreateCollection("bev");



            //var col = db.GetCollection<string>("bev");

            //col.InsertOne

            //col.InsertMany(Specs, new InsertManyOptions { BypassDocumentValidation = true });

            var result = repo.GetItemsAsync(x => true).Result;

            // return await repo.GetItemsAsync(x => true);

            return result;

      
        }

        
    }
}
