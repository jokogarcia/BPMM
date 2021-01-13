import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColeccionHomeComponent } from './coleccion-home.component';

describe('ColeccionHomeComponent', () => {
  let component: ColeccionHomeComponent;
  let fixture: ComponentFixture<ColeccionHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColeccionHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColeccionHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
