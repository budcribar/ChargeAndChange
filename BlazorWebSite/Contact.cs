using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCWebSite.Controllers
{
    public enum ContactStatus { Uncontacted, Contacted, Member, Administrator }

    public class Contact
    {
        public Boolean? Subscriber { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Password { get; set; }
        public string HashedPassword { get; set; }
        public DateTime DateUpdated { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ContactStatus? Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subdivision { get; set; }
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Notes { get; set;  }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    

    public class LarimerCountyRecord
    {
        //https://apps.larimer.org/api/assessor/property/?prop=property&parcel=undefined&scheduleNumber=undefined&serialIdentification=undefined&name=&fromAddrNum=undefined&toAddrNum=undefined&address=&city=FORT%20COLLINS&subdivisionNumber=undefined&sales=any&subdivisionName=WILLOW%20SPRINGS%20PUD
        public int? zipcode { get; set; }
        public string locationcity { get; set; }
        public string locationaddress { get; set; }
        public string ownername1 { get; set; }
        public string proptype { get; set; }

        public string FirstCharToUpper(string value)
        {
            char[] array = value.ToLower().ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }



        public Contact ToContact(string subdivision)
        {
            Contact c;
            try
            {
                c =new Contact { DateUpdated = DateTime.Now, Id = Guid.NewGuid().ToString(), Status= ContactStatus.Uncontacted, City = FirstCharToUpper(locationcity), StreetNumber = int.Parse(locationaddress.Split(' ')[0]), Street = FirstCharToUpper(locationaddress.Substring(locationaddress.IndexOf(' '))).Trim(), State = "CO", ZipCode = zipcode, Subdivision = subdivision, FirstName = FirstCharToUpper(ownername1.Split(' ')[1]), LastName = FirstCharToUpper(ownername1.Split(' ')[0]) };

            }
            catch (Exception) { return null;  }

            return c;
        }
    }



}
