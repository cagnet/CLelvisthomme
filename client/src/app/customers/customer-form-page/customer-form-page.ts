import { ChangeDetectionStrategy, Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiError } from '../../core/api-error';
import { CustomerService } from '../customer-service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-customer-form-page',
  styleUrl: './customer-form-page.css',
  templateUrl: './customer-form-page.html',
})
export class CustomerFormPage implements OnInit {
  // Route param, bound by withComponentInputBinding (undefined on /customers/new)
  id = input<string>();

  customerService = inject(CustomerService);
  fb = inject(FormBuilder);
  router = inject(Router);

  isEdit = computed(() => this.id() != null)
  loading = signal(false)
  saving = signal(false)
  error = signal<string | null>(null)
  serverErrors = signal<Record<string, string[]>>({})

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required]],
    firstName: [''],
    email: [''],
    address: [''],
    isActive: [true]
  })

  ngOnInit(): void {
    const id = this.id();
    if (id == null)
      return
    this.loading.set(true)
    this.customerService.getById(Number(id)).subscribe({
      next: (customer) => {
        this.form.setValue({
          name: customer.name,
          firstName: customer.firstName ?? '',
          email: customer.email ?? '',
          address: customer.address ?? '',
          isActive: customer.isActive
        })
        this.loading.set(false)
      },
      error: (error: ApiError) => {
        this.error.set(error.message)
        this.loading.set(false)
      }
    })
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName)
    return !!(field?.invalid && (field.dirty || field.touched)) || this.fieldErrors(fieldName).length > 0
  }

  fieldErrors(fieldName: string): string[] {
    return this.serverErrors()[fieldName] ?? []
  }

  onSubmit(): void {
    this.form.markAllAsTouched()
    if (this.form.invalid)
      return

    this.saving.set(true)
    this.error.set(null)
    this.serverErrors.set({})

    const id = this.id()
    const customer = this.form.getRawValue()
    const request = id == null
      ? this.customerService.create(customer)
      : this.customerService.update(Number(id), customer)

    request.subscribe({
      next: (saved) => this.router.navigate(['/customers', saved.id]),
      error: (error: ApiError) => {
        this.serverErrors.set(error.fieldErrors)
        this.error.set(error.message)
        this.saving.set(false)
      }
    })
  }
}
