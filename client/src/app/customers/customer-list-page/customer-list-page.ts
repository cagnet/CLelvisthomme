import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiError } from '../../core/api-error';
import { Customer, deleteCustomerErrorMessage } from '../customer';
import { CustomerService } from '../customer-service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  selector: 'app-customer-list-page',
  styleUrl: './customer-list-page.css',
  templateUrl: './customer-list-page.html',
})
export class CustomerListPage implements OnInit {
  customerService = inject(CustomerService);
  customers = computed(() => this.customerService.customers())
  loading = computed(() => this.customerService.loading())
  error = computed(() => this.customerService.error())

  deleteError = signal<string | null>(null)

  ngOnInit(): void {
    this.customerService.getCustomers()
  }

  deleteCustomer(customer: Customer): void {
    if (!confirm(`Delete customer ${customer.name}?`))
      return
    this.deleteError.set(null)
    this.customerService.delete(customer.id).subscribe({
      error: (error: ApiError) => this.deleteError.set(deleteCustomerErrorMessage(customer, error))
    })
  }
}
