import { Injectable } from '@angular/core';
import {AngularFireDatabase} from '@angular/fire/database'
import { AngularFirestore } from '@angular/fire/firestore';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private db: AngularFirestore) { }

  
  addEvent(value)
  {
    
    return this.db.collection('event').add({
      name: value.name
    })
  }
}
