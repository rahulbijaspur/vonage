import { Component, ElementRef, AfterViewInit, ViewChild, Input } from '@angular/core';
import * as OT from '@opentok/client';

@Component({
  selector: 'app-subscriber',
  templateUrl: './subscriber.component.html',
  styleUrls: ['./subscriber.component.css']
})

export class SubscriberComponent implements AfterViewInit {
  @ViewChild('subscriberDiv', { static: true }) subscriberDiv:any= ElementRef;
  @Input() session:any= OT.Session;
  @Input() stream:any= OT.Stream;

  constructor() { }

  ngAfterViewInit() {
    const subscriber = this.session.subscribe(this.stream, this.subscriberDiv.nativeElement, {}, (err:any) => {
      if (err) {
        alert(err.message);
      }
    });
  }
}
