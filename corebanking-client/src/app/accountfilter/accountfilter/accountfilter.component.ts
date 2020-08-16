import { Component, OnInit, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-accountfilter',
  templateUrl: './accountfilter.component.html',
  styleUrls: ['./accountfilter.component.scss']
})
export class AccountfilterComponent implements OnInit {

  @Output() filterChange = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
  }
  
  accountTypeValueChanged(event: any) {
    this.filterChange.emit(event.target.value);
  }
}
