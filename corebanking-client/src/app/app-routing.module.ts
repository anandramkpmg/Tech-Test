import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountComponent} from 'src//app//account/account.component'

const routes: Routes = [
  { path: '', component: AccountComponent },
  { path: 'customers', component: AccountComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
