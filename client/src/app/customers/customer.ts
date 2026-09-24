import { ApiError } from '../core/api-error';

export interface Customer {
    id: number;
    name: string;
    firstName: string | null;
    email: string | null;
    address: string | null;
    isActive: boolean;
}

export interface CreateCustomer {
    name: string;
    firstName?: string;
    email?: string;
    address?: string;
    isActive?: boolean;
}

export interface UpdateCustomer {
    name?: string;
    firstName?: string;
    email?: string;
    address?: string;
    isActive?: boolean;
}

export function deleteCustomerErrorMessage(customer: Customer, error: ApiError): string {
    if (error.code == 'customer_has_orders')
        return `${customer.name} cannot be deleted because they still have orders. Delete their orders first.`;
    return error.message;
}
