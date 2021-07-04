import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JoinComponent } from './join/join.component';
import { SubscriberComponent } from './subscriber/subscriber.component';
import { VideoComponent } from './video/video.component';


const routes: Routes = [
  { path: '', component: JoinComponent, pathMatch: 'full' },
  { path: 'video', component: VideoComponent, pathMatch: 'full' },
  { path: 'subscriber', component: SubscriberComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
