import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompleteToggle } from './complete-toggle';

describe('CompleteToggle', () => {
  let component: CompleteToggle;
  let fixture: ComponentFixture<CompleteToggle>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompleteToggle],
    }).compileComponents();

    fixture = TestBed.createComponent(CompleteToggle);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
