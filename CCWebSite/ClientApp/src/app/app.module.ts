import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
//import { HttpClientModule } from '@angular/common/http';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSortModule } from '@angular/material/sort';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MissionComponent,
    AboutComponent,
    BEVComponent,
    LegalComponent,
    NewslettersComponent,
    FlyersComponent,
    NewsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    //HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatSortModule,

    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'mission', component: MissionComponent },
      { path: 'about', component: AboutComponent },
      { path: 'legal', component: LegalComponent },
      { path: 'newsletters', component: NewslettersComponent },
      { path: 'flyers', component: FlyersComponent },

    ])
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports:[RouterModule]
})
export class AppModule { }

