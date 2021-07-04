import { Component, ElementRef, AfterViewInit, ViewChild, Input } from '@angular/core';
import { OpentokService } from '../opentok.service';
import { ToastrService } from 'ngx-toastr';

const publish = () => {

};

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.css']
})

export class PublisherComponent implements AfterViewInit {
  @ViewChild('publisherDiv', { static: true }) publisherDiv:any= ElementRef;
  @ViewChild('screen', { static: false }) screen:any= ElementRef;
  @Input() session:any= OT.Session;
  publisher:any= OT.Publisher;
  publishing: Boolean;
  ffWhitelistVersion = '36';
  pubish:any;
  publisher2:any;
  // const publish = () => {

  // };

  constructor(private opentokService: OpentokService, private toastr: ToastrService,) {
    this.publishing = false;
  }

  ngAfterViewInit() {
    const OT = this.opentokService.getOT();
    this.publisher = OT.initPublisher(this.publisherDiv.nativeElement, {insertMode: 'append'});

    if (this.session) {
      if (this.session['isConnected']()) {
        this.publish();
      }
      this.session.on('sessionConnected', () => this.publish());
    }
  }

  publish() {
    this.session.publish(this.publisher, (err:any) => {
      if (err) {
        alert(err.message);
      } else {
        this.publishing = true;
      }
    });
  }



  publish1(){
    this.publisherDiv.nativeElement.style.height = '100%';
    this.publisherDiv.nativeElement.style.width = '100%';
    this.publisher = OT.initPublisher(this.publisherDiv.nativeElement,
        {
            audioSource: this.opentokService.audioId,
            videoSource: this.opentokService.videoId,
           
        }, (error) => {
            if (error) {
                // console.log(error)
                this.opentokService.tokboxpopup.next(true);
                if (error.name === 'OT_USER_MEDIA_ACCESS_DENIED') {
                  
                    this.toastr.warning('Please allow access to the Camera and Microphone and try publishing again.', 'Alert');
                } else {
                    this.toastr.warning('Failed to get access to your camera or microphone. Please check that your webcam'
                        + ' is connected and not being used by another application and try again.','Alert');
                }
                this.publisher.destroy();
                this.publisher = null;
                return;
            }
        }
    )
    
    if (this.session != null || this.session != undefined) {
        this.session.publish(this.publisher, (error:any) => {
      
            if(error.name==="OT_NOT_CONNECTED") {
                this.toastr.warning("Publishing your video failed. You are not connected to the internet.");
            } else if (error.name==="OT_CREATE_PEER_CONNECTION_FAILED") {
                this.toastr.warning("Publishing your video failed. This could be due to a restrictive firewall.")
            } else {
                this.toastr.warning("An unknown error occurred while trying to publish your video. Please try again later." + error)
            }
            this.publisher.destroy();
            this.publisher = null;
          
                this.restartPublisher();
                this.opentokService.tokboxpopup.next(true);
         
            return;
        });
        this.session.on("streamCreated", (event:any)=>{
            
        })
        this.session.on("streamDestroyed", (event:any)=>{
          
            this.toastr.info('We are connecting you back..', 'Info')
        })
    }


}

restartPublisher(){
    if (this.session != null || this.session != undefined) {
        this.session.publish(this.publisher, (error:any) => {
            // console.log(error)
            if(error.name==="OT_NOT_CONNECTED") {
                this.toastr.warning("Publishing your video failed. You are not connected to the internet.");
            } else if (error.name==="OT_CREATE_PEER_CONNECTION_FAILED") {
                this.toastr.warning("Publishing your video failed. This could be due to a restrictive firewall.")
            } else {
                this.toastr.warning("An unknown error occurred while trying to publish your video. Please try again later." + error)
            }
            this.publisher.destroy();
            this.publisher = null;
            if(this.opentokService.retry < 2){
                this.restartPublisher();
                this.opentokService.tokboxpopup.next("true");
               



            }else if(this.opentokService.retry = 2){
                
                this.opentokService.tokboxpopup.next("false");
                this.opentokService.retry = 0;
            }

            return;
        });
        this.session.on("streamCreated", (event:any) => {
           
        })
        this.session.on("streamDestroyed", (event:any) => {
          
            this.toastr.info('We are connecting you back..', 'Info')
        })
    }
}

publish2() {
    this.publisher2 = OT.initPublisher('screen', { facingMode: 'user', videoSource: 'screen', name: 'Screen' });
    this.session.publish(this.publisher2, (err:any) => {
    });
}


shareScreen() {
    OT.registerScreenSharingExtension('chrome', 'iccnofgpgpgbgjgfmkmedglffjgldmlg', 2);
    OT.checkScreenSharingCapability((response) => {
        console.info("tokbox  ",response);
        if (!response.supported || response.extensionRegistered === false) {
            alert('This browser does not support screen sharing.');
        } else if (response.extensionInstalled === false
              && (response.extensionRequired || !this.ffWhitelistVersion)) {
                alert('Please install the screen-sharing extension from Chrome Store and load this page over HTTPS://.');
        } 
        // else if (this.ffWhitelistVersion && navigator.userAgent.match(/Firefox/)
        //     && navigator.userAgent.match(/Firefox\/(\d+)/)[1] < this.ffWhitelistVersion) {
        //       alert('For screen sharing, please update your version of Firefox to '
        //         + this.ffWhitelistVersion  + this.ffWhitelistVersion + '.');
        // }
        else{
            this.publish2();
        }
   });
   
//     var mediaDevices1 = await navigator.mediaDevices as any;
//     mediaDevices1.getDisplayMedia().then(mediaStream => {
//         setTimeout(() => {
//           const tracks = mediaStream.getTracks()
//           this.session.localParticipant.publishTrack(tracks[0]);
//         }, 5000)
//     })
// }

// }

 }
 }
