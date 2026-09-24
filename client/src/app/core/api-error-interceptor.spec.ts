import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiError } from './api-error';
import { apiErrorInterceptor } from './api-error-interceptor';

describe('apiErrorInterceptor', () => {
  let httpClient: HttpClient;
  let mockHttp: HttpTestingController;
  let error: ApiError | undefined;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([apiErrorInterceptor])),
        provideHttpClientTesting()
      ]
    });
    httpClient = TestBed.inject(HttpClient);
    mockHttp = TestBed.inject(HttpTestingController);
    error = undefined;
    httpClient.get('/test').subscribe({ error: (e: ApiError) => error = e });
  });

  afterEach(() => {
    mockHttp.verify()
  })

  it('should map 400 validation errors to camelCase field errors', () => {
    mockHttp.expectOne('/test').flush(
      { title: 'One or more validation errors occurred.', status: 400, errors: { Name: ['Name is required.'] } },
      { status: 400, statusText: 'Bad Request' }
    )

    expect(error?.status).toEqual(400);
    expect(error?.fieldErrors).toEqual({ name: ['Name is required.'] });
  })

  it('should map 409 conflicts to code and message', () => {
    mockHttp.expectOne('/test').flush(
      { code: 'inactive_customer', detail: 'Cannot create an order for an inactive customer.' },
      { status: 409, statusText: 'Conflict' }
    )

    expect(error?.code).toEqual('inactive_customer');
    expect(error?.message).toEqual('Cannot create an order for an inactive customer.');
    expect(error?.fieldErrors).toEqual({});
  })

  it('should give a readable message on 404', () => {
    mockHttp.expectOne('/test').flush(null, { status: 404, statusText: 'Not Found' })

    expect(error?.message).toEqual('The requested resource was not found.');
  })
});
