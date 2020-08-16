import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountfilterComponent } from './accountfilter.component';

describe('AccountfilterComponent', () => {
  let component: AccountfilterComponent;
  let fixture: ComponentFixture<AccountfilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountfilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountfilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
