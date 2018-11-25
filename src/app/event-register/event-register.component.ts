import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AngularFirestore, AngularFirestoreDocument } from '@angular/fire/firestore';
import { IEvent } from '../interface/ievent';
import { EventService } from '../service/event.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-event-register',
  templateUrl: './event-register.component.html',
  styleUrls: ['./event-register.component.css']
})
export class EventRegisterComponent implements OnInit {
  sub: any;
  id: string;
  eventa: AngularFirestoreDocument<IEvent>;
  test: IEvent;
  item

  constructor(private route: ActivatedRoute, private db: AngularFirestore, private eventservice: EventService) { 
    
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
    })
    this.setEvent()
  }

  setEvent() {

    var eventRef = this.db.collection('event').doc(this.id);
    var getDoc = eventRef.get()
      .subscribe(doc => {
        if (!doc.exists) {
          console.log('No such document!');
        } else {
          console.log('Document data:', doc.data())
          this.test = doc.data() as IEvent
          console.log(this.test.name)
        }
      },
      error=>
      console.log(error))
    
  }

  ConfirmRegistration(numofpeople: number)
  {
   this.eventservice.emrollUser(this.id,numofpeople)
   console.log(this.item)
  }

}
