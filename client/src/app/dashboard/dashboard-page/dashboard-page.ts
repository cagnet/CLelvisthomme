import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { ApiError } from '../../core/api-error';
import { Customer } from '../../customers/customer';
import { CustomerService } from '../../customers/customer-service';
import { Order } from '../../orders/order';
import { OrderService } from '../../orders/order-service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CurrencyPipe],
  selector: 'app-dashboard-page',
  styleUrl: './dashboard-page.css',
  templateUrl: './dashboard-page.html',
})
export class DashboardPage implements OnInit {
  customerService = inject(CustomerService);
  orderService = inject(OrderService);

  customers = signal<Customer[]>([])
  orders = signal<Order[]>([])
  loading = signal(false)
  error = signal<string | null>(null)

  activeCount = computed(() => this.customers().filter((v) => v.isActive).length)
  inactiveCount = computed(() => this.customers().length - this.activeCount())
  orderCount = computed(() => this.orders().length)
  totalAmount = computed(() => this.orders().reduce((total, order) => total + order.amount, 0))
  averageAmount = computed(() => this.orderCount() == 0 ? 0 : this.totalAmount() / this.orderCount())

  ngOnInit(): void {
    this.loading.set(true)
    forkJoin({
      customers: this.customerService.getAll(),
      orders: this.orderService.getAll()
    }).subscribe({
      next: ({ customers, orders }) => {
        this.customers.set(customers)
        this.orders.set(orders)
        this.loading.set(false)
      },
      error: (error: ApiError) => {
        this.error.set(error.message)
        this.loading.set(false)
      }
    })
  }
}
