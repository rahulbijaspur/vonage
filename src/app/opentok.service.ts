import { Injectable } from '@angular/core';

import * as OT from '@opentok/client';
import config from '../config';
import { BehaviorSubject, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class OpentokService {
  public tokboxpopup = new BehaviorSubject<any>("");
  public screenShareStop = new BehaviorSubject<any>("");
  retry = 0;
  session:any= OT.Session;
  token:string="fljkasdf";
  public  videoId: any = undefined;
  public  audioId: any = undefined;
  sessionId: string="";
 

  constructor(private http:HttpClient) { }

  getOT() {
    return OT;
  }

  initSession() {
    this.http.post("https://localhost:5001/Session/CreateSession?username=rahul","").subscribe(res => {
      this.sessionId =res.toString();
    });
    this.http.post("https://localhost:5001/Session/GetToken?username=rahul&sessionId="+this.sessionId,"").subscribe(res => {
      this.token=res.toString();
    });
    if (config.API_KEY && this.token && this.sessionId) {
      this.session = this.getOT().initSession(config.API_KEY, this.sessionId);
      this.token = this.token;
      console.log(this.token);
      return Promise.resolve(this.session);
    } else {
      return fetch(config.SAMPLE_SERVER_BASE_URL + '/session')
        .then((data) => data.json())
        .then((json) => {
          this.session = this.getOT().initSession(json.apiKey, json.sessionId);
          this.token = json.token;
          return this.session;
        });
    }
  }

  connect() {
    return new Promise((resolve, reject) => {
      this.session.connect(this.token, (err:any) => {
        if (err) {
          reject(err);
        } else {
          resolve(this.session);
        }
      });
    });
  }
}
