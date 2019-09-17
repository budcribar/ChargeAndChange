import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes,RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MissionComponent } from './mission/mission.component';
import { LegalComponent } from './legal/legal.component';
import { NewslettersComponent } from './newsletters/newsletters.component';
import { FlyersComponent } from './flyers/flyers.component';
import { AboutComponent } from './about/about.component';
import { NewsComponent } from './news/news.component';

import { BEVComponent } from './bev/bev.component';
import { SigninComponent } from './signin/signin.component';
import { SignoutComponent } from './signout/signout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSortModule } from '@angular/material/sort';
import { MatToolbarModule, MatButtonModule, MatFormFieldModule,MatSelectModule, MatInputModule, MatIconModule, MatTableModule, MatDialogModule, MatDatepickerModule, MatGridListModule, MatRadioModule, MatCheckboxModule, MatHeaderCell } from '@angular/material';
import { ConfirmationDialogComponent } from './confirmationDialog/confirmationDialog.component';
import { BEVEditorComponent } from './bevEditor/bevEditor.component';
import { ContactComponent } from './contact/contact.component';
import { ContactEditorComponent } from './contactEditor/contactEditor.component';
import { ChartModule } from 'angular-highcharts';
import { BEVChartComponent } from './bevChart/bevChart.component';
import { Globals } from './globals';
import { SocialLoginModule, AuthServiceConfig, FacebookLoginProvider } from 'angularx-social-login';

const config = new AuthServiceConfig([
  {
    id: FacebookLoginProvider.PROVIDER_ID,
    provider: new FacebookLoginProvider('447198995878594')
  }
]);

export function provideConfig() {
  return config;
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MissionComponent,
    AboutComponent,
    BEVComponent,
    BEVEditorComponent,
    ContactComponent,
    ContactEditorComponent,
    ConfirmationDialogComponent,
    LegalComponent,
    NewslettersComponent,
    FlyersComponent,
    NewsComponent,
    SigninComponent,
    SignoutComponent,
   
    BEVChartComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    SocialLoginModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatSortModule,
    MatSelectModule,
    MatToolbarModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatDialogModule,
    MatDatepickerModule,
    MatGridListModule,
    MatRadioModule,
    ChartModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'mission', component: MissionComponent },
      { path: 'about', component: AboutComponent },
      { path: 'legal', component: LegalComponent },
      { path: 'newsletters', component: NewslettersComponent },
      { path: 'flyers', component: FlyersComponent },
      { path: 'bevs', component: BEVComponent },
      { path: 'contacts', component: ContactComponent },
      { path: 'login', component: SigninComponent },
      { path: 'logout', component: SignoutComponent }
    ])
  ],
  providers: [Globals,
    {
      provide: AuthServiceConfig,
      useFactory: provideConfig
    }],
  bootstrap: [AppComponent],
  exports: [RouterModule],
  entryComponents:[ConfirmationDialogComponent,BEVEditorComponent,ContactEditorComponent]
})
export class AppModule { }

