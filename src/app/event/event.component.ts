import { Component, OnInit } from '@angular/core';
import { IEvent } from '../interface/ievent';
import { Router } from '@angular/router';
import { EventService } from '../service/event.service';
import { AngularFirestore, AngularFirestoreCollection } from '@angular/fire/firestore';
import { map } from 'rxjs/operators'
import { Observable } from 'rxjs';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

  eventsCollection: AngularFirestoreCollection<Event>;
  events: Event[];

  constructor(public db: AngularFirestore, eventservice: EventService,
    private router: Router) { }

  ngOnInit() {
    this.eventsCollection = this.db.collection('event')

    this.eventsCollection.valueChanges().subscribe(data => {
      this.events = data
    })
  }
}
