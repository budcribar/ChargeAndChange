using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public interface IBEVViewModel
    {
        bool IsBusy { get; set; }
        bool IsSmallDisplay { get; set; }
        int PowerMode { get; set; } 
        int PriceWeighting { get; set; }
        int RangeWeighting { get; set; }
        int ChargeWeighting { get; set; }
        string[] DriveTypes { get; }
        string[] BodyStyles { get; }
        string[] BodyStyleSelections { get; }
        string[] DriveTypeSelections { get; }
        string[] ManufacturerSelections { get; }
        string[] ConnectorTypes { get; }
        List<EVSpecs> BevSpecs { get; }
        EVSpecs[] SortedSpecs { get; }
        DataItem[] FilteredSpecs { get; }
        public IEnumerable<(string Value, string Text)> Specs { get; }
        string SelectedSpec { get; set; }
        string SelectedBodyStyle { get; set; }
        string SelectedManufacturer { get; set; }
        string SelectedDriveType { get; set; }
        Task OnInitialized ();
        Task DeleteRow(string id);
        Task InsertRow();
        Task UpdateRow(EVSpecs specs);
        int? GetMotorPower(EVSpecs spec);
        // TODO Rename
        void ChangeMotorPower(int? value, EVSpecs spec);
        void ChangeBodyStyle(string value, EVSpecs spec);
        void ChangeDriveType(string value, EVSpecs spec);
        void ChangeConnectorType(string value, EVSpecs spec);
        event PropertyChangedEventHandler? PropertyChanged;
        Func<object, string> SpecFormat { get; }
    }
}