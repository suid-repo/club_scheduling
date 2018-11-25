import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { EventService } from '../service/event.service';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.css']
})
export class EventCreateComponent implements OnInit {
  form: FormGroup;
  

  constructor(private eventService: EventService) {
    this.form = new FormGroup({
      name: new FormControl('')
    });
   }

  ngOnInit() {
  }

  createevent ()
  {
    console.log(this.form.value)
    this.eventService.addEvent(this.form.value)
	.then(
	  res => {
	    
	    
	  })
  }
}
