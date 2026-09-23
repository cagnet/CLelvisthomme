CREATE TABLE Customer
(
    Id       int IDENTITY PRIMARY KEY,
    Name     nvarchar(100) NOT NULL,
    IsActive bit NOT NULL
);

CREATE TABLE [Order]
(
    Id         int IDENTITY PRIMARY KEY,
    CustomerId int NOT NULL,
    Amount     decimal(18,2) NOT NULL,
    CreatedAt  datetime2 NOT NULL,
    CONSTRAINT CK_Order_Amount CHECK (Amount > 0),
    CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);

CREATE INDEX IX_Order_CustomerId ON [Order](CustomerId);

-- La règle « client actif » doit rester dans l'application : une FK ne peut pas
-- exprimer de manière fiable une condition portant sur Customer.IsActive.
