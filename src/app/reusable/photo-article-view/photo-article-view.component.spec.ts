import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoArticleViewComponent } from './photo-article-view.component';

describe('PhotoArticleViewComponent', () => {
  let component: PhotoArticleViewComponent;
  let fixture: ComponentFixture<PhotoArticleViewComponent>;

  beforeEach(async(() => {
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
