import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SociosHomeComponent } from './socios-home.component';

describe('SociosHomeComponent', () => {
  let component: SociosHomeComponent;
  let fixture: ComponentFixture<SociosHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SociosHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SociosHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
