import { Injectable } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database'
import { AngularFirestore, AngularFirestoreCollection } from '@angular/fire/firestore';
import { AngularFireAuth } from '@angular/fire/auth';
import * as firebase from 'firebase';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'
import { IEvent } from '../interface/ievent';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private db: AngularFirestore, firebase: AngularFireAuth) {

    this.tasks = db.collection<Event>('event');
  }
  tasks: AngularFirestoreCollection<Event>;
  users: Observable<Event>


  

  GetEvents() {

    
  }

  addEvent(value) {

    return this.db.collection('event').add({
      name: value.name,
      date: value.date,
      time: value.time,
      createdBy: firebase.auth().currentUser.uid
    })
  }

  emrollUser(eventID:string,numofpeople: number){
    this.db.collection('event/'+eventID+'/members').add({
      bookedby: firebase.auth().currentUser.uid,
      
    })
  }
}

