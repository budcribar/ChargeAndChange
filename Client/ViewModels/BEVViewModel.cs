using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Client.ViewModels
{

    public partial class BEVViewModel : BaseViewModel, IBEVViewModel
    {
        private const string ALL = "<all>";
        private SwaggerClient client;

        public BEVViewModel(SwaggerClient client)
        {
            this.client = client;
        }
        private string[] driveTypeSelections = new List<string> { ALL }.Concat(Enum.GetNames(typeof(DriveType))).ToArray();
        public string[] DriveTypeSelections => driveTypeSelections;

        private string[] bodyStyleSelections = new List<string> { ALL }.Concat(Enum.GetNames(typeof(BodyStyle))).ToArray();
        public string[] BodyStyleSelections => bodyStyleSelections;
        public string[] ManufacturerSelections => new List<string> { ALL }.Concat(BevSpecs.Select(x => x.Manufacturer).Distinct()).ToArray();
        public string[] DriveTypes => Enum.GetNames(typeof(DriveType));
        public string[] BodyStyles => Enum.GetNames(typeof(BodyStyle));
        public string[] ConnectorTypes => Enum.GetNames(typeof(ChargingConnector));

        public bool IsSmallDisplay { get; set; }

        private List<EVSpecs> bevSpecs = new List<EVSpecs>();
        public List<EVSpecs> BevSpecs
        {
            get => bevSpecs;
            set
            {
                SetValue(ref bevSpecs, value);
            }
        }



        Dictionary<string, (string Name, Func<EVSpecs, double?>, Func<object, string>)> specs = new Dictionary<string, (string, Func<EVSpecs, double?>, Func<object, string>)> {
            {"Price", ("Price", (e) => (double?) e.Price, (e) => ((double)e).ToString("C0") )  },

            {  "PriceMinusFederalTaxCredit",  ("Price After Tax Credit", (e) => (double?) (e.Price - e.FederalTaxCredit), (e) => ((double)e).ToString("C0") )  },
            {  "CombinedRange",  ("Range", (e) => (double?) e.CombinedRange, (e) => $"{((double)e)} mi" ) },
            {  "MotorPowerHp",  ("Motor (hp)", (e) => (double?) (e.MotorPowerKw * 1.34102), (e) => $"{((double)e)} hp")  },
            {  "Torque", ( "Torque", (e) => (double?) e.Torque, (e) => $"{((double)e)} ft-lbs") },
            {  "BatteryCapacity",  ("Batery Capacity (kwh)", (e) => (double?) e.BatteryCapacity, (e) => $"{((double)e)} kwh") },
            {  "Weight",  ("Weight (lbs)", (e) => (double?) e.Weight, (e) => $"{((double)e)} lbs") },
            {  "ZeroTo60mph",  ("Zero To 60 mph", (e) => (double?) e.ZeroTo60mph, (e) => $"{((double)e)} sec") },
            {  "MaxChargePower",  ("Max Charge Power (kw)", (e) => (double?) e.MaxChargePower, (e) => $"{((double)e)} kw") },
            {  "MinutesTo80PercentCharge",  ("Minutes To 80% Charge", (e) => (double?) e.MinutesTo80PercentCharge, (e) => $"{((double)e)} min") },
            {  "PricePerMiRange",  ("Price Per Mile of Range", (e) => (double?) (e.Price / e.CombinedRange), (e) => ((double)e).ToString("C0") )  },
            {  "Efficiency",  ("wh Per Mile", (e) => (double?) (e.BatteryCapacity * 1000.0 / e.CombinedRange), (e) => $"{(Math.Round((double)e))} wh/mi") },
            {  "SafetyRating",  ("Safety Rating", (e) => (double?) e.SafetyRating, (e) => $"{((double)e)} stars") } };

        public IEnumerable<(string Value, string Text)> Specs => specs.Keys.Select(x => (x, specs[x].Item1));

        public string SelectedSpecName => specs[SelectedSpec].Name;

        private string selectedSpec = "";
        public string SelectedSpec
        {
            get => selectedSpec;
            set
            {
                SetValue(ref selectedSpec, value);
                FilterSpecs();
            }
        }

        private string selectedBodyStyle = ALL;
        public string SelectedBodyStyle
        {
            get => selectedBodyStyle;
            set
            {
                SetValue(ref selectedBodyStyle, value);
                FilterSpecs();
            }
        }
        
        private string selectedManufacturer = ALL;
        public string SelectedManufacturer
        {
            get => selectedManufacturer;
            set
            {
                SetValue(ref selectedManufacturer, value);
                FilterSpecs();
            }
        }

        private string selectedDriveType = ALL;
        public string SelectedDriveType
        {
            get => selectedDriveType;
            set
            {
                SetValue(ref selectedDriveType, value);
                FilterSpecs();
            }
        }

        private void FilterSpecs()
        {
            var allSpecs = BevSpecs.ToList();

            if (selectedManufacturer != ALL)
                allSpecs = allSpecs.Where(x => x.Manufacturer == selectedManufacturer).ToList();

            if (selectedBodyStyle != ALL)
                allSpecs = allSpecs.Where(x => x.BodyStyle.ToString() == selectedBodyStyle).ToList();

            if (selectedDriveType != ALL)
                allSpecs = allSpecs.Where(x => x.DriveTrain.ToString() == selectedDriveType).ToList();

            FilteredSpecs = allSpecs.Where(x => specs[selectedSpec].Item2.Invoke(x) != null).OrderBy(x => specs[selectedSpec].Item2.Invoke(x)).Select(x => new DataItem { Name = x.Model, Value = specs[selectedSpec].Item2.Invoke(x), Manufacturer=x.Manufacturer }).ToArray();      
        }

        public Func<object, string> SpecFormat => specs[selectedSpec].Item3;

        public DataItem[] FilteredSpecs { get; private set; } = Array.Empty<DataItem>();
        public EVSpecs[] SortedSpecs { get {
                var allSpecs = BevSpecs.ToList();

                if (selectedManufacturer != ALL)
                    allSpecs = allSpecs.Where(x => x.Manufacturer == selectedManufacturer).ToList();

                if (selectedBodyStyle != ALL)
                    allSpecs = allSpecs.Where(x => x.BodyStyle.ToString() == selectedBodyStyle).ToList();

                if (selectedDriveType != ALL)
                    allSpecs = allSpecs.Where(x => x.DriveTrain.ToString() == selectedDriveType).ToList();

                allSpecs = allSpecs.Where(x => x.Available && x.CombinedRange != null && x.Price != null && x.MaxChargePower != null && x.ZeroTo60mph != null).ToList();

                if (RangeWeighting < 5 && PriceWeighting < 5 && ChargeWeighting < 5 && PerformanceWeighting < 5)
                    return allSpecs.OrderBy(x => x.Manufacturer).ToArray();

                var sortedRange = allSpecs.Select(x => x.CombinedRange).OrderBy(x => x).ToList(); // higher has higher index
                var sortedPrice = allSpecs.Select(x => x.Price).OrderByDescending(x => x).ToList(); // lower has higher index
                var sortedCharge = allSpecs.Select(x => x.MaxChargePower).OrderBy(x => x).ToList(); // higher has higher index
                var sortedPerformance = allSpecs.Select(x => x.ZeroTo60mph).OrderByDescending(x => x).ToList(); // lower has higher index
                var rangeInc = (3 * RangeWeighting) / 100.0;
                var priceInc = (3 * PriceWeighting) / 100.0;  
                var chargeInc = (3 * ChargeWeighting) / 100.0;  
                var performanceInc = (3 * PerformanceWeighting) / 100.0;
                return allSpecs.OrderByDescending(x => sortedRange.IndexOf(x.CombinedRange) * rangeInc + sortedPrice.IndexOf(x.Price) * priceInc + sortedCharge.IndexOf(x.MaxChargePower) * chargeInc + sortedPerformance.IndexOf(x.ZeroTo60mph) * performanceInc).ToArray();
            
            } }

        public int PowerMode { get; set; } = 1;

        private int performanceWeighting = 0;
        public int PerformanceWeighting
        {
            get => performanceWeighting;
            set
            {
                SetValue(ref performanceWeighting, value);
                FilterSpecs();
            }
        }
        private int priceWeighting = 0;
        public int PriceWeighting
        {
            get => priceWeighting;
            set
            {
                SetValue(ref priceWeighting, value);
                FilterSpecs();
            }
        }
        private int rangeWeighting = 0;
        public int RangeWeighting
        {
            get => rangeWeighting;
            set
            {
                SetValue(ref rangeWeighting, value);
                FilterSpecs();
            }
        }
        private int chargeWeighting = 0;
        public int ChargeWeighting
        {
            get => chargeWeighting;
            set
            {
                SetValue(ref chargeWeighting, value);
                FilterSpecs();
            }
        }

        public int? GetMotorPower(EVSpecs spec)
        {
            if (spec.MotorPowerKw == null) return null;
            if (PowerMode == 1) return spec.MotorPowerKw;
            return (int) Math.Round(spec.MotorPowerKw.Value * 1.34102);
        }

        public void ChangeMotorPower(int? value, EVSpecs spec)
        {
            int v = value == null ? 0 : (int)value;
            bool isKw = PowerMode == 1;
            spec.MotorPowerKw = isKw ? value : (int)Math.Round(v * .7457);
            PowerMode = 1;
            OnPropertyChanged("MotorPower");
        }
        public void ChangeDriveType(string value, EVSpecs spec)
        {
            spec.DriveTrain = Enum.Parse<DriveType>(value);

            OnPropertyChanged("DriveType");
        }

        public void ChangeConnectorType(string value, EVSpecs spec)
        {
            spec.ChargingConnector = Enum.Parse<ChargingConnector>(value);

            OnPropertyChanged("ConnectorType");
        }

        public void ChangeBodyStyle(string value, EVSpecs spec)
        {
            spec.BodyStyle = Enum.Parse<BodyStyle>(value);

            OnPropertyChanged("BodyStyle");
        }

        private async Task LoadSpecs()
        {
            BevSpecs = (await client.EVSpecsAsync()).ToList().OrderBy(x => x.Manufacturer).ThenBy(x => x.Model).ToList();
        }

        
        public async Task UpdateRow(EVSpecs evSpecs)
        {
            await client.PatchEVSpecsAsync(evSpecs.Id, evSpecs);        
        }
      
        public async Task DeleteRow(string id)
        {
            await client.DeleteEVSpecsAsync(id);

            await LoadSpecs();
        }
        public async Task InsertRow()
        {
            EVSpecs spec = new EVSpecs();
            spec.Model = "New model";
            spec.ModelYear = DateTime.Now.Year;

            await client.PostEVSpecsAsync(spec);

            await LoadSpecs();
        }

        public override async Task OnInitialized()
        {
            await LoadSpecs();
            bodyStyleSelections = new List<string> { ALL }.Concat(BevSpecs.Select(x => x.BodyStyle.ToString()).Distinct()).ToArray();
            driveTypeSelections = new List<string> { ALL }.Concat(BevSpecs.Select(x => x.DriveTrain.ToString()).Distinct()).ToArray();
            SelectedSpec = specs.Keys.First();
            selectedBodyStyle = ALL;
            selectedDriveType = ALL;
            selectedManufacturer = ALL;
            PerformanceWeighting = 0;
            PriceWeighting = 0;
            RangeWeighting = 0;
            ChargeWeighting = 0;
        }

    }
}
