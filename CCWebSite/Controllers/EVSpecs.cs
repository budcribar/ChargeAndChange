using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCWebSite.Controllers
{
    public enum BodyStyle { SUV,Sedan, Crossover, Pickup, Coupe, Minivan, Van, Wagon, Hatchback, Convertible }
    public enum DriveType {  AWD, FOURWD, FWD, RWD }
    public enum ChargingConnector {  Chademo, CCS, Tesla }
    public enum MotorPowerUnits {  kw, hp }
    public class EVSpecs
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public int ModelYear { get; set; }
        [JsonProperty(PropertyName = "manufacturer")]
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BodyStyle? BodyStyle { get; set; }
        public decimal? Price { get; set; }
        public decimal? FederalTaxCredit { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DriveType? DriveTrain {get;set; }
        public int? CombinedRange { get; set; }
        public int? CityRange { get; set; }
        public int? HiwayRange { get; set; }
        public int? MotorPowerKw { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MotorPowerUnits? MotorPowerUnits { get; set; }
        public decimal? PricePerMileOfRange { get; set;  }
        public int? Torque { get; set; }

        public int? BatteryCapacity { get; set;  }
        [JsonConverter(typeof(StringEnumConverter))]
        public ChargingConnector? ChargingConnector { get; set;  }
        public int? Weight { get; set;  }
        public double? ZeroTo60mph { get; set; }
        public double? ZeroTo100kph { get; set; }
        public int? MaxChargePower { get; set;  }
        public int? MinutesTo80PercentCharge { get; set;  }
        public int? SafetyRating { get; set; }

        public string Notes { get; set;  }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    

    
}
