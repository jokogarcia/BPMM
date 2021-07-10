import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SociosNotaComponent } from './socios-nota.component';

describe('SociosNotaComponent', () => {
  let component: SociosNotaComponent;
  let fixture: ComponentFixture<SociosNotaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SociosNotaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SociosNotaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
