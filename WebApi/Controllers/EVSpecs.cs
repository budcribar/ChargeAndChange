﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApi.Controllers
{
    public enum BodyStyle { SUV,Sedan, Crossover, Pickup, Coupe, Minivan, Van, Wagon, Hatchback, Convertible, Unknown }
    public enum DriveType {  AWD, FOURWD, FWD, RWD, Unknown }
    public enum ChargingConnector {  Chademo, CCS, Tesla, Unknown }
    public enum MotorPowerUnits {  kw, hp }
    public class EVSpecs
    {
        [JsonProperty(PropertyName = "id")] // Must be lower case for database
        public string Id { get; set; } = "";
        public DateTime DateUpdated { get; set; }
        public int? ModelYear { get; set; }
        public string Manufacturer { get; set; } = "";
        public Boolean Available { get; set; }
        public string Model { get; set; } = "";
        [JsonConverter(typeof(StringEnumConverter))]
        public BodyStyle BodyStyle { get; set; } = BodyStyle.Unknown;
        public decimal? Price { get; set; }
        public decimal? FederalTaxCredit { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DriveType DriveTrain { get; set; } = DriveType.Unknown;
        public int? CombinedRange { get; set; }
        public int? CityRange { get; set; }
        public int? HiwayRange { get; set; }
        public int? MotorPowerKw { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public MotorPowerUnits MotorPowerUnits { get; set; } = Controllers.MotorPowerUnits.kw;
       
        public int? Torque { get; set; }

        public double? BatteryCapacity { get; set;  }
       
        [JsonConverter(typeof(StringEnumConverter))]
        public ChargingConnector ChargingConnector { get; set;  }
        public int? Weight { get; set;  }
        public double? ZeroTo60mph { get; set; }
        public double? ZeroTo100kph { get; set; }
        public int? MaxChargePower { get; set;  }
        public int? MinutesTo80PercentCharge { get; set;  }
        public int? SafetyRating { get; set; }

        public string? Notes { get; set;  }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    

    
}
