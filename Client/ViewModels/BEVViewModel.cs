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

        public string[] DriveTypeSelections => new List<string> { ALL }.Concat(Enum.GetNames(typeof(DriveType))).ToArray(); 
        public string[] BodyStyleSelections => new List<string> { ALL }.Concat(Enum.GetNames(typeof(BodyStyle))).ToArray();
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



        Dictionary<string, (string, Func<EVSpecs, double?>, Func<object, string>)> specs = new Dictionary<string, (string, Func<EVSpecs, double?>, Func<object, string>)> {
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

        public string selectedSpec = "";
        public string SelectedSpec
        {
            get => selectedSpec;
            set
            {
                SetValue(ref selectedSpec, value);
                FilterSpecs();
            }
        }

        public string selectedBodyStyle = ALL;
        public string SelectedBodyStyle
        {
            get => selectedBodyStyle;
            set
            {
                SetValue(ref selectedBodyStyle, value);
                FilterSpecs();
            }
        }
        
        public string selectedManufacturer = ALL;
        public string SelectedManufacturer
        {
            get => selectedManufacturer;
            set
            {
                SetValue(ref selectedManufacturer, value);
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

            FilteredSpecs = allSpecs.Where(x => specs[selectedSpec].Item2.Invoke(x) != null).OrderBy(x => specs[selectedSpec].Item2.Invoke(x)).Select(x => new DataItem { Name = x.Model, Value = specs[selectedSpec].Item2.Invoke(x) }).ToArray();      
        }

        public Func<object, string> SpecFormat => specs[selectedSpec].Item3;

        public DataItem[] FilteredSpecs { get; private set; } = Array.Empty<DataItem>();

        public int PowerMode { get; set; } = 1;

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
            SelectedSpec = specs.Keys.First();
        }

    }
}
