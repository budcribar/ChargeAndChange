using System.ComponentModel;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public interface IContactViewModel
    {
        Contact[] FilteredContacts { get; }
        string[] InterestTypes { get; }
        string SelectedStreet { get; set; }
        string SelectedSubdivision { get; set; }
        string[] StatusTypes { get; }
        string?[] Streets { get; set; }
        string?[] Subdivisions { get; set; }

        void ChangeContactStatus(string value, Contact spec);
        void ChangeEVInterest(string value, Contact spec);
        void ChangeLawnEquipmentInterest(string value, Contact spec);
        Task DeleteRow(Contact contact);
        Task InsertRow();
        Task OnInitialized();
        Task UpdateRow(Contact contact);
        event PropertyChangedEventHandler? PropertyChanged;
    }
}