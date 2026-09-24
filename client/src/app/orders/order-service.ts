import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CUSTOMER_URL } from '../customers/customer-service';
import { CreateOrder, Order, UpdateOrder } from './order';

export const ORDER_URL = `${environment.baseUrl}/orders`;

@Service()
export class OrderService {
    httpClient = inject(HttpClient);

    getAll(): Observable<Order[]> {
        return this.httpClient.get<Order[]>(ORDER_URL);
    }

    getByCustomer(customerId: number): Observable<Order[]> {
        return this.httpClient.get<Order[]>(`${CUSTOMER_URL}/${customerId}/orders`);
    }

    create(customerId: number, order: CreateOrder): Observable<Order> {
        return this.httpClient.post<Order>(`${CUSTOMER_URL}/${customerId}/orders`, order);
    }

    update(id: number, order: UpdateOrder): Observable<Order> {
        return this.httpClient.patch<Order>(`${ORDER_URL}/${id}`, order);
    }

    delete(id: number): Observable<void> {
        return this.httpClient.delete<void>(`${ORDER_URL}/${id}`);
    }
}
