import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ToastrModule,ToastContainerModule  } from 'ngx-toastr';  
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { PublisherComponent } from './publisher/publisher.component';
import { SubscriberComponent } from './subscriber/subscriber.component';
import { OpentokService } from './opentok.service';


@NgModule({
  declarations: [
    AppComponent,
    PublisherComponent,
    SubscriberComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastContainerModule,
    HttpClientModule,
    ToastrModule.forRoot()  
  ],
  providers: [OpentokService],
  bootstrap: [AppComponent]
})
export class AppModule {
 }
