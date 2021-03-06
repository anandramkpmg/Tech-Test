import { Component, OnInit } from '@angular/core';
import { Observable, Subject, BehaviorSubject } from "rxjs";
import { shareReplay } from "rxjs/operators";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AccountService } from "../services/account.service";
import { AccountModel } from '../models/account-model.model';
import { combineLatest } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  filter$: Subject<string>;
  createForm: FormGroup;
  accounts$: Observable<AccountModel[]>;
  filteredaccounts$: Observable<AccountModel[]>;
  errormessage: string;

  constructor(private readonly formBuilder: FormBuilder,
    private readonly accountService: AccountService) { }

  ngOnInit(): void {
    this.createForm = this.formBuilder.group({
      firstName: this.formBuilder.control("", [Validators.required, Validators.minLength(3)]),
      lastName: this.formBuilder.control("", [Validators.required, Validators.minLength(3)]),
      balance: this.formBuilder.control("", [Validators.required]),
    });

    this.createForm.valueChanges.subscribe(selectedValue => {
      console.log('form value changed')
      console.log(selectedValue)

      this.errormessage = null;

      var accountExists = this.isAccountExists(selectedValue.firstName, selectedValue.lastName);

      if (accountExists) {
        this.errormessage = "User names already exists";
      }
    })

    this.filter$ = new BehaviorSubject('All');
    this.errormessage = null;
    this.getAllAccounts();
  }

  isAccountExists(firstName: string, lastName: string) {
    var userExists = false;
    this.accounts$.forEach(element => {
      var matchingAccount = element.filter(
        (account: AccountModel) =>
          account.firstName.toLowerCase() === firstName.toLowerCase() &&
          account.lastName.toLowerCase() === lastName.toLowerCase()
      )
      return userExists = matchingAccount.length > 0;
    });
    return userExists;
  }

  createAccount() {
    if (this.createForm.invalid) {
      alert('Form Invalid');
    } else {
      this.accountService
        .CreateAccount(this.createForm.value)
        .subscribe(() => this.getAllAccounts(), (err) => {
          if (err instanceof HttpErrorResponse && err.status == 500) {
            this.errormessage = "Server Error : Internal Server Error.";
          }
          else {
            this.errormessage = err.message;
          }
        });
    }
  }

  getAllAccounts() {
    this.accounts$ = this.accountService
      .GetAccounts()
      .pipe(shareReplay({ bufferSize: 1, refCount: true }));

    this.filteredaccounts$ = this.createFilterAccounts(
      this.filter$,
      this.accounts$);
  }

  filterChanged(value: string) {
    this.filter$.next(value);
  }

  public createFilterAccounts(
    filter$: Observable<string>,
    accounts$: Observable<AccountModel[]>) {
    return combineLatest(
      accounts$,
      filter$, (account: AccountModel[], filter: string) => {

        if (filter === "All") return account;
        return account.filter(
          (account: AccountModel) =>
            account.accountType.toLowerCase()
            === filter.toLowerCase()
        );
      });
  }
}
