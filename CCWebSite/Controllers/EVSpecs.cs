using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCWebSite.Controllers
{
    public enum BodyStyle { SUV,Sedan, Crossover, Truck, Hatchback }
    public enum DriveType {  Front, Rear, AWD }
    public enum ChargingConnector {  Chademo, CCS, Tesla }
    public class EVSpecs
    {
        public DateTime DateUpdated { get; set; }
        public int ModelYear { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyStyle? BodyStyle { get; set; }
        public decimal? Price { get; set; }
        public decimal? FederalTaxCredit { get; set; }
        public DriveType? Drive {get;set; }
        public int? CombinedRange { get; set; }
        public int? CityRange { get; set; }
        public int? HiwayRange { get; set; }
        public int? MotorPowerKw { get; set; }
        public decimal? PricePerMileOfRange { get; set;  }
        public int? Torque { get; set; }

        public int? BatteryCapacity { get; set;  }
        public ChargingConnector? ChargingConnector { get; set;  }
        public int? Weight { get; set;  }
        public double? ZeroTo60 { get; set; }
        public double? ZeroTo62 { get; set; }
        public int? MaxChargePower { get; set;  }
        public int? MinutesTo80PercentCharge { get; set;  }
        public string Notes { get; set;  }
    }
    

    
}
