using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   
    public class BEVViewModel : BaseViewModel, IBEVViewModel
    {
        private SwaggerClient client;

        public BEVViewModel(SwaggerClient client)
        {
            this.client = client;
        }

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

        public async Task OnInit()
        {
            await LoadSpecs();
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
    }
}
