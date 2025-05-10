# RESTful Web API with .NET 8

A RESTful Web API built with C# and .NET 8.0 that integrates with the mock API provided at [https://restful-api.dev](https://restful-api.dev/). This API extends the functionality of the mock API by adding filtering, pagination, input validation, and robust error handling.

## Features

- Retrieve products from the mock API with filtering by name and pagination
- Add new products to the mock API
- Delete products via the mock API
- Proper model validation with meaningful error messages
- Global exception handling middleware
- Comprehensive Swagger documentation

## Technical Overview

This project demonstrates integration with a third-party API while adding value through:

- Filtering capabilities (product name substring matching)
- Pagination for better performance and UX
- Data validation using data annotations
- Robust error handling with custom exception types
- Clean architecture separation of concerns

## Prerequisites

- .NET 8.0 SDK

## Project Structure

- **Controllers/**: API endpoints
  - `ProductsController.cs`: Handles HTTP requests and responses

- **Models/**: Data models
  - `Product.cs`: Core data model representing a product

- **DTOs/**: Data Transfer Objects for request/response shaping
  - `ProductDto.cs`: DTOs for various product operations
  - `ProductSearchParams.cs`: Parameters for product filtering and pagination

- **Services/**: Business logic and external API interactions
  - `IProductService.cs`: Service interface definition
  - `ProductService.cs`: Implementation that communicates with restful-api.dev

- **Exceptions/**: Custom exception types
  - `ApiException.cs`: Base and specialized exception classes

- **Middleware/**: Request/response pipeline components
  - `ExceptionHandlingMiddleware.cs`: Global exception handler

## Setup and Run

1. Clone this repository
2. Navigate to the project directory
3. Build the project:
   ```sh
   dotnet build
   ```
4. Run the API:
   ```sh
   dotnet run
   ```
5. The API will be available at:
   - http://localhost:5142
   - Swagger UI is available at http://localhost:5142/swagger

## API Endpoints

### Get Products
```
GET /api/products?nameFilter={nameFilter}&page={page}&pageSize={pageSize}
```
- `nameFilter` (optional): Filter products by name (substring match)
- `page` (optional, default=1): Page number for pagination
- `pageSize` (optional, default=10): Number of items per page

**Response:**
```json
{
  "products": [
    {
      "id": 1,
      "name": "Product Name",
      "data": {
        "year": 2023,
        "price": 199.99,
        "CPU model": "Intel Core i5",
        "Hard disk size": "512GB",
        "color": "silver"
      }
    }
  ],
  "totalCount": 25,
  "page": 1,
  "pageSize": 10
}
```

### Get Product by ID
```
GET /api/products/{id}
```

**Response:**
```json
{
  "id": 1,
  "name": "Product Name",
  "data": {
    "year": 2023,
    "price": 199.99,
    "CPU model": "Intel Core i5",
    "Hard disk size": "512GB",
    "color": "silver"
  }
}
```

### Create Product
```
POST /api/products
```

**Request Body:**
```json
{
  "name": "New Laptop",
  "data": {
    "year": 2025,
    "price": 1299.99,
    "CPU model": "AMD Ryzen 9",
    "Hard disk size": "2TB",
    "color": "black"
  }
}
```

**Response:**
```json
{
  "id": 12,
  "name": "New Laptop",
  "data": {
    "year": 2025,
    "price": 1299.99,
    "CPU model": "AMD Ryzen 9",
    "Hard disk size": "2TB",
    "color": "black"
  }
}
```

### Delete Product
```
DELETE /api/products/{id}
```

**Response:** HTTP 204 No Content

## Error Handling

The API includes comprehensive error handling:

- **400 Bad Request**: Validation errors with details
  ```json
  {
    "statusCode": 400,
    "message": "Name is required"
  }
  ```

- **404 Not Found**: Resource not found
  ```json
  {
    "statusCode": 404,
    "message": "Product with ID 999 not found"
  }
  ```

- **500 Internal Server Error**: Unexpected errors
  ```json
  {
    "statusCode": 500,
    "message": "An unexpected error occurred."
  }
  ```

## Validation

The API performs validation on all inputs:

- Product name is required and cannot exceed 100 characters
- Product year must be between 1900 and 2100
- Product price must be greater than zero
- Page number must be greater than 0
- Page size must be between 1 and 100