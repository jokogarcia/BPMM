import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComisionComponent } from './comision.component';

describe('ComisionComponent', () => {
  let component: ComisionComponent;
  let fixture: ComponentFixture<ComisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
