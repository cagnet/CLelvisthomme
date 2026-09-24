import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { apiErrorInterceptor } from '../../core/api-error-interceptor';
import { Customer } from '../customer';
import { CUSTOMER_URL } from '../customer-service';
import { CustomerListPage } from './customer-list-page';

describe('CustomerListPage', () => {
  let component: CustomerListPage;
  let fixture: ComponentFixture<CustomerListPage>;
  let httpMock: HttpTestingController;

  const mockCustomers: Customer[] = [
    { id: 1, name: 'Dupont', firstName: 'Jean', email: null, address: null, isActive: true },
    { id: 2, name: 'Martin', firstName: 'Claire', email: null, address: null, isActive: false },
    { id: 3, name: 'Durand', firstName: null, email: null, address: null, isActive: true }
  ]

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerListPage],
      providers: [
        provideHttpClient(withInterceptors([apiErrorInterceptor])),
        provideHttpClientTesting(),
        provideRouter([])
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerListPage);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    await fixture.whenStable();
  });

  afterEach(() => {
    httpMock.verify();
  })

  const loadCustomers = (customers: Customer[]) => {
    httpMock.expectOne(CUSTOMER_URL).flush(customers);
    fixture.detectChanges();
  }

  it('should show a loader while customers are loading', () => {
    expect(fixture.nativeElement.querySelector('.loading')).toBeTruthy();
    loadCustomers(mockCustomers);
    expect(fixture.nativeElement.querySelector('.loading')).toBeFalsy();
  })

  it('should render one row per customer with its status', () => {
    loadCustomers(mockCustomers);

    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toEqual(3);
    expect(rows[1].textContent).toContain('Martin');
    expect(rows[1].textContent).toContain('Inactive');
  })

  it('should render the empty state when there is no customer', () => {
    loadCustomers([]);

    expect(fixture.nativeElement.textContent).toContain('No customer yet');
  })

  it('should filter customers by name and first name, case insensitive', () => {
    loadCustomers(mockCustomers);

    component.search.set('  jEAn ');
    expect(component.filteredCustomers().map((c) => c.id)).toEqual([1]);

    component.search.set('N');
    expect(component.filteredCustomers().map((c) => c.id)).toEqual([1, 2, 3]);
  })

  it('should combine the search with the status filter', () => {
    loadCustomers(mockCustomers);

    component.statusFilter.set('inactive');
    expect(component.filteredCustomers().map((c) => c.id)).toEqual([2]);

    component.statusFilter.set('active');
    component.search.set('dur');
    expect(component.filteredCustomers().map((c) => c.id)).toEqual([3]);
  })

  it('should show a specific message when no customer matches the search', () => {
    loadCustomers(mockCustomers);
    component.search.set('unknown');
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('No customer matches your search');
  })

  it('should display a clear message when the API refuses the delete (409)', () => {
    loadCustomers(mockCustomers);
    vi.spyOn(window, 'confirm').mockReturnValue(true);

    component.deleteCustomer(mockCustomers[0]);
    httpMock.expectOne(`${CUSTOMER_URL}/1`).flush(
      { code: 'customer_has_orders', detail: 'A customer with orders cannot be deleted.' },
      { status: 409, statusText: 'Conflict' }
    );
    fixture.detectChanges();

    const alert = fixture.nativeElement.querySelector('[role="alert"]');
    expect(alert.textContent).toContain('Dupont cannot be deleted because they still have orders');
    expect(fixture.nativeElement.querySelectorAll('tbody tr').length).toEqual(3);
  })
});
