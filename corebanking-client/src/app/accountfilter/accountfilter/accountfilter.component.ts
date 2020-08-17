import { Component, OnInit, Output, EventEmitter} from '@angular/core';
import { Observable, Subject, BehaviorSubject } from "rxjs";
import { AccountService } from "src/app/services/account.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

import { shareReplay } from "rxjs/operators";

@Component({
  selector: 'app-accountfilter',
  templateUrl: './accountfilter.component.html',
  styleUrls: ['./accountfilter.component.scss']
})
export class AccountfilterComponent implements OnInit {

  @Output() filterChange = new EventEmitter<string>();
  accountTypes$: Observable<string[]>;
  accountTypes: FormGroup;
  constructor(    private readonly accountService: AccountService,private fb: FormBuilder) { }

  ngOnInit() {
    this.accountTypes = this.fb.group({
      control: ['NA']
    });
    this.getAccountTypes();
   }

  getAccountTypes() {
    this.accountTypes$ = this.accountService
      .GetAccountTypes()
      .pipe(shareReplay({ bufferSize: 1, refCount: true }))
  }
  
  accountTypeValueChanged(event: any) {
    this.filterChange.emit(event.target.value);
  }
}
