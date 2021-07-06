import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SociosSolicitudComponent } from './socios-solicitud.component';

describe('SociosSolicitudComponent', () => {
  let component: SociosSolicitudComponent;
  let fixture: ComponentFixture<SociosSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SociosSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SociosSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
