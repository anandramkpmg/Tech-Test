import { Component, OnInit } from '@angular/core';
import { Observable , Subject, BehaviorSubject} from "rxjs";
import { shareReplay } from "rxjs/operators";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AccountService } from "../services/account.service"
import { AccountModel } from '../models/account-model.model';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  filter$: Subject<string>;
  public createForm: FormGroup;
  public accounts$: Observable<AccountModel[]>;
  filteredaccounts$: Observable<AccountModel[]>;


  constructor(private readonly formBuilder: FormBuilder,
    private readonly accountService: AccountService) { }

  ngOnInit(): void {
    this.createForm = this.formBuilder.group({
      firstName: this.formBuilder.control("", [Validators.required, Validators.minLength(3)]),
      lastName: this.formBuilder.control("", [Validators.required, Validators.minLength(3)]),
      balance: this.formBuilder.control("", [Validators.required]),
    });

    this.filter$ = new BehaviorSubject('All');

    this.getAllAccounts();

    // this.filteredaccounts$ = this.createFilterCharacters(
    //   this.filter$,
    //   this.accounts$
//);
  }

  createAccount() {
    if (this.createForm.invalid) {
      alert('Form Invalid');
    } else {
      this.accountService
        .CreateAccount(this.createForm.value)
        .subscribe(() => this.getAllAccounts());
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
