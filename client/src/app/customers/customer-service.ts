import { inject, Service, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiError } from '../core/api-error';
import { CreateCustomer, Customer, UpdateCustomer } from './customer';

export const CUSTOMER_URL = `${environment.baseUrl}/customers`;

@Service()
export class CustomerService {
    customers = signal<Customer[]>([]);
    loading = signal<boolean>(false);
    error = signal<string | null>(null);

    httpClient = inject(HttpClient);

    getCustomers(): void {
        this.loading.set(true);
        this.httpClient.get<Customer[]>(CUSTOMER_URL).subscribe(
            {
                next: (value) => {
                    this.customers.set(value);
                    this.error.set(null);
                    this.loading.set(false);
                },
                error: (error: ApiError) => {
                    this.error.set(error.message);
                    this.loading.set(false);
                }
            }
        )
    }

    getById(id: number): Observable<Customer> {
        return this.httpClient.get<Customer>(`${CUSTOMER_URL}/${id}`);
    }

    create(customer: CreateCustomer): Observable<Customer> {
        return this.httpClient.post<Customer>(CUSTOMER_URL, customer);
    }

    update(id: number, customer: UpdateCustomer): Observable<Customer> {
        return this.httpClient.patch<Customer>(`${CUSTOMER_URL}/${id}`, customer);
    }

    delete(id: number): Observable<void> {
        return this.httpClient.delete<void>(`${CUSTOMER_URL}/${id}`).pipe(
            tap(() => this.customers.update((v) => v.filter((customer) => customer.id != id)))
        );
    }
}
