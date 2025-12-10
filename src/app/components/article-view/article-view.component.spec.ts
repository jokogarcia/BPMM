import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ArticleViewComponent } from './article-view.component';

describe('ArticleViewComponent', () => {
  let component: ArticleViewComponent;
  let fixture: ComponentFixture<ArticleViewComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ArticleViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticleViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
