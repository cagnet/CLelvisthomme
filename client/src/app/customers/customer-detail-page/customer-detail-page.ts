import { ChangeDetectionStrategy, Component, inject, input, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ApiError } from '../../core/api-error';
import { Order } from '../../orders/order';
import { OrderService } from '../../orders/order-service';
import { Customer, deleteCustomerErrorMessage } from '../customer';
import { CustomerService } from '../customer-service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, RouterLink, CurrencyPipe, DatePipe],
  selector: 'app-customer-detail-page',
  styleUrl: './customer-detail-page.css',
  templateUrl: './customer-detail-page.html',
})
export class CustomerDetailPage implements OnInit {
  // Route param, bound by withComponentInputBinding
  id = input.required<string>();

  customerService = inject(CustomerService);
  orderService = inject(OrderService);
  fb = inject(FormBuilder);
  router = inject(Router);

  customer = signal<Customer | null>(null)
  orders = signal<Order[]>([])
  loading = signal(false)
  error = signal<string | null>(null)
  deleteError = signal<string | null>(null)

  editingOrderId = signal<number | null>(null)
  savingOrder = signal(false)
  orderError = signal<string | null>(null)
  orderFieldErrors = signal<Record<string, string[]>>({})

  // No client-side rule on amount: the API validates it and returns a 400
  orderForm = this.fb.group({
    amount: this.fb.control<number | null>(null)
  })

  ngOnInit(): void {
    const id = Number(this.id())
    this.loading.set(true)
    forkJoin({
      customer: this.customerService.getById(id),
      orders: this.orderService.getByCustomer(id)
    }).subscribe({
      next: ({ customer, orders }) => {
        this.customer.set(customer)
        this.orders.set(orders)
        this.loading.set(false)
      },
      error: (error: ApiError) => {
        this.error.set(error.message)
        this.loading.set(false)
      }
    })
  }

  deleteCustomer(customer: Customer): void {
    if (!confirm(`Delete customer ${customer.name}?`))
      return
    this.deleteError.set(null)
    this.customerService.delete(customer.id).subscribe({
      next: () => this.router.navigateByUrl('/customers'),
      error: (error: ApiError) => this.deleteError.set(deleteCustomerErrorMessage(customer, error))
    })
  }

  editOrder(order: Order): void {
    this.clearOrderErrors()
    this.editingOrderId.set(order.id)
    this.orderForm.setValue({ amount: order.amount })
  }

  cancelEdit(): void {
    this.clearOrderErrors()
    this.editingOrderId.set(null)
    this.orderForm.reset()
  }

  saveOrder(): void {
    const editingId = this.editingOrderId()
    const amount = this.orderForm.getRawValue().amount ?? 0
    const request = editingId == null
      ? this.orderService.create(Number(this.id()), { amount })
      : this.orderService.update(editingId, { amount })

    this.clearOrderErrors()
    this.savingOrder.set(true)
    request.subscribe({
      next: (saved) => {
        this.orders.update((v) => editingId == null
          ? [...v, saved]
          : v.map((order) => order.id == saved.id ? saved : order))
        this.savingOrder.set(false)
        this.cancelEdit()
      },
      error: (error: ApiError) => {
        this.orderFieldErrors.set(error.fieldErrors)
        this.orderError.set(error.message)
        this.savingOrder.set(false)
      }
    })
  }

  deleteOrder(order: Order): void {
    if (!confirm(`Delete order #${order.id}?`))
      return
    this.orderError.set(null)
    this.orderService.delete(order.id).subscribe({
      next: () => {
        this.orders.update((v) => v.filter((o) => o.id != order.id))
        if (this.editingOrderId() == order.id)
          this.cancelEdit()
      },
      error: (error: ApiError) => this.orderError.set(error.message)
    })
  }

  private clearOrderErrors(): void {
    this.orderError.set(null)
    this.orderFieldErrors.set({})
  }
}
