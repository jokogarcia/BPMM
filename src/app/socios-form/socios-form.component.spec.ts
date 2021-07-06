import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SociosFormComponent } from './socios-form.component';

describe('SociosFormComponent', () => {
  let component: SociosFormComponent;
  let fixture: ComponentFixture<SociosFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SociosFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SociosFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
