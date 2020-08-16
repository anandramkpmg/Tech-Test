import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountModel } from 'src/app/models/account-model.model'

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  public CreateAccount(account: AccountModel): Observable<AccountModel> {
    return this.httpClient.post<AccountModel>(`${environment.apiEndpoint}/account`, account);
  }

  public GetAccounts(): Observable<AccountModel[]> {
    return this.httpClient.get<AccountModel[]>(`${environment.apiEndpoint}/account`);
  }

}
