export interface Order {
    id: number;
    customerId: number;
    amount: number;
    createdAt: string;
}

export interface CreateOrder {
    amount: number;
}

export interface UpdateOrder {
    amount?: number;
}
