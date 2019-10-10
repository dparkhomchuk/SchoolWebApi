import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchooltableComponent } from './schooltable.component';

describe('SchooltableComponent', () => {
  let component: SchooltableComponent;
  let fixture: ComponentFixture<SchooltableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchooltableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchooltableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
