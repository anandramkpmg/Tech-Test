import { Accounttype } from '../models/accounttype.model';

export interface AccountModel {
    id: number,
    firstName: string,
    lastName: string,
    balance: number,
    accountType: Accounttype
}
