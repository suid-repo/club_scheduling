import { Component, OnInit } from '@angular/core';
import { IEvent } from '../interface/ievent';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  
  event:IEvent;

  constructor() { }

  ngOnInit() {
  }

}
