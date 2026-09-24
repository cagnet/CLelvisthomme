import { TestBed } from '@angular/core/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { apiErrorInterceptor } from '../core/api-error-interceptor';
import { ApiError } from '../core/api-error';
import { Customer } from './customer';
import { CUSTOMER_URL, CustomerService } from './customer-service';

describe('CustomerService', () => {
  let service: CustomerService;
  let mockHttp: HttpTestingController;

  const mockCustomers: Customer[] = [
    { id: 1, name: 'Dupont', firstName: 'Jean', email: 'jean@dupont.fr', address: null, isActive: true },
    { id: 2, name: 'Martin', firstName: null, email: null, address: null, isActive: false }
  ]

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([apiErrorInterceptor])),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(CustomerService);
    mockHttp = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    mockHttp.verify()
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch customers and update signals on success', () => {
    service.getCustomers();
    expect(service.loading()).toEqual(true);

    const req = mockHttp.expectOne(CUSTOMER_URL)
    expect(req.request.method).toEqual('GET')
    req.flush(mockCustomers);

    expect(service.customers()).toEqual(mockCustomers);
    expect(service.loading()).toEqual(false);
    expect(service.error()).toBeNull();
  })

  it('should set a readable error when the API is unreachable', () => {
    service.getCustomers();
    mockHttp.expectOne(CUSTOMER_URL).error(new ProgressEvent('error'), { status: 0 });

    expect(service.loading()).toEqual(false);
    expect(service.error()).toContain('Unable to reach the API');
  })

  it('should POST the customer on create', () => {
    const body = { name: 'Durand', firstName: '', email: '', address: '', isActive: true };
    service.create(body).subscribe((customer) => expect(customer.id).toEqual(3));

    const req = mockHttp.expectOne(CUSTOMER_URL)
    expect(req.request.method).toEqual('POST')
    expect(req.request.body).toEqual(body)
    req.flush({ ...body, id: 3 })
  })

  it('should PATCH the customer on update', () => {
    service.update(1, { isActive: false }).subscribe();

    const req = mockHttp.expectOne(`${CUSTOMER_URL}/1`)
    expect(req.request.method).toEqual('PATCH')
    expect(req.request.body).toEqual({ isActive: false })
    req.flush({ ...mockCustomers[0], isActive: false })
  })

  it('should remove the customer from the list on delete', () => {
    service.customers.set(mockCustomers);
    service.delete(1).subscribe();

    const req = mockHttp.expectOne(`${CUSTOMER_URL}/1`)
    expect(req.request.method).toEqual('DELETE')
    req.flush(null, { status: 204, statusText: 'No Content' })

    expect(service.customers().map((c) => c.id)).toEqual([2]);
  })

  it('should keep the customer and expose the conflict when delete is refused', () => {
    service.customers.set(mockCustomers);
    let error: ApiError | undefined;
    service.delete(1).subscribe({ error: (e: ApiError) => error = e });

    mockHttp.expectOne(`${CUSTOMER_URL}/1`).flush(
      { code: 'customer_has_orders', detail: 'A customer with orders cannot be deleted.' },
      { status: 409, statusText: 'Conflict' }
    )

    expect(error?.code).toEqual('customer_has_orders');
    expect(service.customers().length).toEqual(2);
  })
});
