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
