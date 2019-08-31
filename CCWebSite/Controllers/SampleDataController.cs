using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CCWebSite.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static List<EVSpecs> Specs = new List<EVSpecs>
        {
            new EVSpecs {DateUpdated=new DateTime(2019,30,8), ModelYear=2019, Manufacturer="Audi", Model = "E-Tron", BodyStyle=null, Price=74800,FederalTaxCredit=null, Drive=null, CombinedRange=204, CityRange=null, HiwayRange=null, MotorPowerKw=265, Torque=414, BatteryCapacity=95, ChargingConnector=null, Weight=null, ZeroTo60=5.5, ZeroTo62=null, MaxChargePower=150, MinutesTo80PercentCharge=30, Notes="" },
            new EVSpecs {DateUpdated=new DateTime(2019,31,8), ModelYear=2019, Manufacturer="BMW", Model = "i3", BodyStyle=null, Price=44450,FederalTaxCredit=null, Drive=null, CombinedRange=153, CityRange=null, HiwayRange=null, MotorPowerKw=(int)Math.Round(170*.7457), Torque=null, BatteryCapacity=null, ChargingConnector=ChargingConnector.CCS, Weight=null, ZeroTo60=7.2, ZeroTo62=null, MaxChargePower=50, MinutesTo80PercentCharge=40, Notes="" },
              new EVSpecs {DateUpdated=new DateTime(2019,31,8), ModelYear=2019, Manufacturer="BMW", Model = "MINI Cooper", BodyStyle=null, Price=null,FederalTaxCredit=null, Drive=null, CombinedRange=null, CityRange=null, HiwayRange=null, MotorPowerKw=null, Torque=null, BatteryCapacity=null, ChargingConnector=null, Weight=null, ZeroTo60=null, ZeroTo62=null, MaxChargePower=null, MinutesTo80PercentCharge=null, Notes="" },

        };

        [HttpGet("[action]")]
        public IEnumerable<EVSpecs> EVSpecs()
        {
            return Specs;
        }

        
    }
}
