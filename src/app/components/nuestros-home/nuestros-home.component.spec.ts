import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { NuestrosHomeComponent } from './nuestros-home.component';

describe('NuestrosHomeComponent', () => {
  let component: NuestrosHomeComponent;
  let fixture: ComponentFixture<NuestrosHomeComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ NuestrosHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NuestrosHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
