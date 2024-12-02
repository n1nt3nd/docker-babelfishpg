using Microsoft.Data.SqlClient;

Console.WriteLine("Starting SQL script execution...");

string username = "babelfish_user";
string password = "12345678";
string connectionString = $"Server=localhost,1433;User Id={username};Password={password};Encrypt=False;";


string sqlScript = @"
-- Create schema
DROP SCHEMA IF EXISTS example_db;
CREATE SCHEMA example_db;
GO

-- Drop existing tables if any
DROP TABLE IF EXISTS example_db.orders;
DROP TABLE IF EXISTS example_db.products;
DROP TABLE IF EXISTS example_db.customers;
GO

-- Create tables
CREATE TABLE example_db.customers (
    customer_id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE example_db.products (
    product_id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE example_db.orders (
    order_id INT IDENTITY PRIMARY KEY,
    customer_id INT NOT NULL,
    order_date DATETIME2 NOT NULL DEFAULT GETDATE(),
    total DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES example_db.customers (customer_id) ON DELETE CASCADE
);
GO

-- Create indexes
CREATE INDEX IX_Customers_Email ON example_db.customers(email);
CREATE INDEX IX_Orders_CustomerId ON example_db.orders(customer_id);
GO

-- Create views
CREATE VIEW example_db.vw_customer_orders AS
SELECT 
    c.customer_id,
    c.name AS customer_name,
    o.order_id,
    o.order_date,
    o.total
FROM 
    example_db.customers c
INNER JOIN 
    example_db.orders o ON c.customer_id = o.customer_id;
GO

-- Create stored procedures
CREATE PROCEDURE example_db.usp_get_customer_orders
    @customer_id INT
AS
BEGIN
    SELECT 
        c.name AS customer_name,
        o.order_id,
        o.order_date,
        o.total
    FROM 
        example_db.customers c
    INNER JOIN 
        example_db.orders o ON c.customer_id = o.customer_id
    WHERE 
        c.customer_id = @customer_id;
END;
GO

-- Seed data
INSERT INTO example_db.customers (name, email) VALUES
    ('John Doe', 'john.doe@example.com'),
    ('Jane Smith', 'jane.smith@example.com');

INSERT INTO example_db.products (name, price) VALUES
    ('Laptop', 999.99),
    ('Smartphone', 699.99);

INSERT INTO example_db.orders (customer_id, total) VALUES
    (1, 999.99),
    (2, 699.99);

";

try
{
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    Console.WriteLine("Connected to the database.");

    var sqlCommands = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var command in sqlCommands)
    {
        using var sqlCommand = new SqlCommand(command.Trim(), connection);
        sqlCommand.ExecuteNonQuery();
        Console.WriteLine("Executed command: " + command.Trim().Split('\n')[0] + "...");
    }

    Console.WriteLine("SQL script executed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

Console.WriteLine("Done.");
