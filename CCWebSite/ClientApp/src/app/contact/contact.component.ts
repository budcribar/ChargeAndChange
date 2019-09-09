import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ApiService, Contact } from '../api.service';
import { ConfirmationDialogComponent, ConfirmationDialogModel } from '../confirmationDialog/confirmationDialog.component';
import { MatDialog } from '@angular/material';
import { ContactEditorComponent } from '../contactEditor/contactEditor.component';

@Component({
  selector: 'app-contact-component',
  templateUrl: './contact.component.html'
})
export class ContactComponent implements OnInit {

  sortedData: Contact[];
  unsortedData: Contact[];
  contact: Contact;
  selectedContact: Contact;
  error: any;

  constructor(private apiService: ApiService, public dialog: MatDialog) {
  }

  ngOnInit() {
    this.apiService.getContacts().subscribe((data) => {
      console.log(data);
      this.sortedData = data.slice();
      this.unsortedData = data.slice();

    })
  }

  addSubdivision() {
    this.apiService.getContactsFrom('WILLOW SPRINGS PUD').subscribe((data) => {

    })
  }

  addContact() {
    
    const dialogRef = this.dialog.open(ContactEditorComponent, { maxWidth: "400px", data: []});

    dialogRef.afterClosed().subscribe(dr => {
      if (dr) {
        dr.dateUpdated = new Date();
        console.log(`Dialog sent: ${JSON.stringify(dr)}`);
       
        this.apiService.insertContact(dr).subscribe(x => {
          this.ngOnInit();
        })
      }

    });
  }

  edit(contact: Contact): void {
    console.log(`contact is ${JSON.stringify(contact)}`);
    
    const dialogRef = this.dialog.open(ContactEditorComponent, { maxWidth: "400px", data: contact });

    dialogRef.afterClosed().subscribe((dr: Contact) => {
      if (dr) {
        dr.dateUpdated = new Date();
        dr.id = contact.id;
        console.log(`Dialog sent: ${JSON.stringify(dr)}`);

        this.apiService.updateContact(dr).subscribe(x => {
          this.ngOnInit();
        })
      }
      
    });
  }

  deleteContact(contact: Contact, event: any): void {
    event.stopPropagation();

    const dialogData = new ConfirmationDialogModel("Confirm Delete", "Are you sure?");
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, { maxWidth: "400px", data: dialogData });

    dialogRef.afterClosed().subscribe(dr => {
      console.log(`Dialog sent: ${dr}`);
      if (dr) {
        this.apiService
          .deleteContact(contact, null).subscribe(x => {
            this.sortedData = this.sortedData.filter(h => h !== contact);
            if (this.selectedContact === contact) { this.selectedContact = null; }
          }, error => this.error = error
          );
      }
    });



  }

  sortData(sort: Sort) {
    const data = this.unsortedData.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'FirstName': return compare(a.firstName, b.firstName, isAsc);
        case 'LastName': return compare(a.lastName, b.lastName, isAsc);

        case 'Subdivision': return compare(a.subdivision, b.subdivision, isAsc);


        case 'StreetNumber': return compare(a.streetNumber, b.streetNumber, isAsc);
        case 'Street': return compare(a.street, b.street, isAsc);
        
        case 'City': return compare(a.city, b.city, isAsc);


        case 'State': return compare(a.state, b.state, isAsc);
        case 'ZipCode': return compare(a.zipCode, b.zipCode, isAsc);



        case 'Email': return compare(a.email, b.email, isAsc);


        case 'Phone': return compare(a.phone, b.phone, isAsc);

        
        case 'Notes': return compare(a.notes, b.notes, isAsc);
        

        default: return 0;
      }
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

