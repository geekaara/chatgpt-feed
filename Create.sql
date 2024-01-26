-- SQL Script to create the database and its tables

-- Creating the Customer table
CREATE TABLE Customer (
    CustomerID INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    TFN NVARCHAR(11),
    Address NVARCHAR(50),
    City NVARCHAR(40),
    State NVARCHAR(3),
    Postcode NVARCHAR(4),
    Mobile NVARCHAR(12),
    PRIMARY KEY (CustomerID)
);

-- Creating Login table
CREATE TABLE Login (
    LoginID CHAR(8) NOT NULL,
    CustomerID INT NOT NULL,
    PasswordHash CHAR(94) NOT NULL,
    PRIMARY KEY (LoginID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);

-- Creating Account table
CREATE TABLE Account (
    AccountNumber INT NOT NULL,
    AccountType CHAR(1) NOT NULL,
    CustomerID INT NOT NULL,
    PRIMARY KEY (AccountNumber),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);

-- Creating Transaction table
CREATE TABLE Transaction (
    TransactionID INT NOT NULL AUTO_INCREMENT,
    TransactionType CHAR(1) NOT NULL,
    AccountNumber INT NOT NULL,
    DestinationAccountNumber INT,
    Amount MONEY NOT NULL,
    Comment NVARCHAR(30),
    TransactionTimeUtc DATETIME2 NOT NULL,
    PRIMARY KEY (TransactionID),
    FOREIGN KEY (AccountNumber) REFERENCES Account(AccountNumber),
    FOREIGN KEY (DestinationAccountNumber) REFERENCES Account(AccountNumber)
);

-- Creating BillPay table
CREATE TABLE BillPay (
    BillPayID INT NOT NULL AUTO_INCREMENT,
    AccountNumber INT NOT NULL,
    PayeeID INT NOT NULL,
    Amount MONEY NOT NULL,
    ScheduleTimeUtc DATETIME2 NOT NULL,
    Period CHAR(1) NOT NULL,
    PRIMARY KEY (BillPayID),
    FOREIGN KEY (AccountNumber) REFERENCES Account(AccountNumber),
    FOREIGN KEY (PayeeID) REFERENCES Payee(PayeeID)
);

-- Creating Payee table
CREATE TABLE Payee (
    PayeeID INT NOT NULL AUTO_INCREMENT,
    Name NVARCHAR(50) NOT NULL,
    Address NVARCHAR(50) NOT NULL,
    City NVARCHAR(40) NOT NULL,
    State NVARCHAR(3) NOT NULL,
    Postcode NVARCHAR(4) NOT NULL,
    Phone NVARCHAR(14) NOT NULL,
    PRIMARY KEY (PayeeID)
);
