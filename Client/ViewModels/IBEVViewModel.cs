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
        string[] DriveTypes { get; }
        string[] BodyStyles { get; }
        string[] ConnectorTypes { get; }
        List<EVSpecs> BevSpecs { get; }
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
    }
}