import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { CustomerFormPage } from './customer-form-page';

describe('CustomerFormPage', () => {
  let component: CustomerFormPage;
  let fixture: ComponentFixture<CustomerFormPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerFormPage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerFormPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
