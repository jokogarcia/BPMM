import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PhotoArticleViewComponent } from './photo-article-view.component';

describe('PhotoArticleViewComponent', () => {
  let component: PhotoArticleViewComponent;
  let fixture: ComponentFixture<PhotoArticleViewComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotoArticleViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotoArticleViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
