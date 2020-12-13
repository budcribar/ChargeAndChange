using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class ContactViewModel : BaseViewModel, IContactViewModel
    {
        private SwaggerClient client;

        public ContactViewModel(SwaggerClient client)
        {
            this.client = client;
        }

        private const string ALL = "<all>";

        private string selectedStreet = ALL;
        public string SelectedStreet
        {
            get => selectedStreet;
            set
            {
                selectedStreet = value;
                Reselect();
                OnPropertyChanged("SelectedStreet");
            }
        }

        private string selectedSubdivision = ALL;
        public string SelectedSubdivision
        {
            get => selectedSubdivision;
            set
            {
                SetValue(ref selectedSubdivision, value);
                SelectedStreet = ALL;
            }
        }

        private Contact[] contacts = Array.Empty<Contact>();
        public Contact[] FilteredContacts { get; private set; } = Array.Empty<Contact>();
        public string?[] Subdivisions { get; set; } = Array.Empty<string?>();
        public string?[] Streets { get; set; } = Array.Empty<string>();

        public string[] StatusTypes { get; } = Enum.GetNames(typeof(ContactStatus));
        public string[] InterestTypes { get; } = Enum.GetNames(typeof(InterestLevel));

        private void BuildSelections()
        {
            var temp = contacts.Select(x => x.Subdivision?.Trim()).Distinct().ToList();
            temp.Insert(0, ALL);
            Subdivisions = temp?.ToArray() ?? Array.Empty<string?>();

            temp = contacts.Select(x => x.Street?.Trim()).Distinct().ToList();
            temp.Insert(0, ALL);
            Streets = temp?.ToArray() ?? Array.Empty<string?>();
        }
        public void ChangeLawnEquipmentInterest(string value, Contact spec)
        {
            spec.LawnEquipmentInterest = Enum.Parse<InterestLevel>(value);

            OnPropertyChanged("LawnEquipmentInterest");
        }
        public void ChangeEVInterest(string value, Contact spec)
        {
            spec.EvInterest = Enum.Parse<InterestLevel>(value);

            OnPropertyChanged("EvInterest");
        }

        public void ChangeContactStatus(string value, Contact spec)
        {
            spec.Status = Enum.Parse<ContactStatus>(value);
            OnPropertyChanged("ContactStatus");
        }

        public void Reselect()
        {
            BuildSelections();

            if (selectedStreet == ALL)
            {
                if (selectedSubdivision == ALL)
                    FilteredContacts = contacts;
                else
                    FilteredContacts = contacts.Where(x => x.Subdivision?.Trim() == selectedSubdivision).ToArray();
            }
            else
            {
                if (selectedSubdivision == ALL)
                    FilteredContacts = contacts.Where(x => x.Street?.Trim() == selectedStreet).ToArray();
                else
                    FilteredContacts = contacts.Where(x => x.Subdivision?.Trim() == selectedSubdivision && x.Street?.Trim() == selectedStreet).ToArray();
            }
        }

        async Task Reload()
        {
            contacts = (await client.ContactsAsync()).OrderBy(x => x.Street).ThenBy(x => x.StreetNumber).ToArray();
            Reselect();
        }

        public async Task InsertRow()
        {
            Contact spec = new Contact();

            await client.PutContactAsync(spec);

            await Reload();
        }

        public async Task DeleteRow(Contact contact)
        {
            await client.DeleteContactAsync(contact.Id);

            await Reload();
        }
        public async Task UpdateRow(Contact contact)
        {
            var old = await client.GetContactByIdAsync(contact.Id);

            contact.HashedPassword = old?.HashedPassword ?? "";

            await client.PatchContactAsync(contact.Id, contact);

            await Reload();
        }
        public override async Task OnInitialized()
        {
            contacts = (await client.ContactsAsync()).OrderBy(x => x.Street).ThenBy(x => x.StreetNumber).ToArray();
            FilteredContacts = contacts;
            BuildSelections();
        }
    }
}
