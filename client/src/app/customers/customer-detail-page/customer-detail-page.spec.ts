import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { CustomerDetailPage } from './customer-detail-page';

describe('CustomerDetailPage', () => {
  let component: CustomerDetailPage;
  let fixture: ComponentFixture<CustomerDetailPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerDetailPage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerDetailPage);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('id', '1');
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
