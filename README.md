# Todo Application API Documentation

## Overview

The **Todo Application** is a RESTful API built using **ASP.NET Core** that allows users to manage their personal tasks (Todos). This application supports user registration, authentication using JWT tokens, and CRUD (Create, Read, Update, Delete) operations for managing Todo items. The API is built with a secure authentication mechanism using JSON Web Tokens (JWT) and stores data in a PostgreSQL database.

### Main Features

1. **User Authentication**:

   - User registration (signup) with unique usernames and emails.
   - User login (signin) with JWT token generation for authenticated access.

2. **Todo Management**:

   - Create new Todo items.
   - Retrieve Todo items by ID or get all Todos for the authenticated user.
   - Update existing Todo items.
   - Delete Todo items.

3. **Security**:
   - Passwords are hashed using BCrypt for secure storage.
   - JWT tokens are used for secure authentication and authorization.

## Project Structure

```
TodoApp/
├── Controllers/
│   ├── AuthController.cs        # Handles user signup, signin, and JWT generation
│   └── TodoController.cs        # Handles CRUD operations for Todo items
├── Data/
│   └── TodoContext.cs           # Entity Framework Core DbContext for database interaction
├── Dtos/
│   └── AuthDto.cs         # Data Transfer Object (DTO) for user registration
│   └── UserSignInDto.cs         # DTO for user login
│   └── CreateTodoDto.cs         # DTO for creating a new Todo item
├── Models/
│   └── User.cs                  # Entity model representing a user
│   └── Todo.cs                  # Entity model representing a Todo item
├── Program.cs                   # Application entry point and middleware configuration
├── appsettings.json             # Configuration file for database connection and JWT settings
├── TodoApp.csproj                # Project file containing dependencies and build settings
```

## Prerequisites

- **.NET 8 SDK**
- **PostgreSQL** database
- **Visual Studio** or **Visual Studio Code**
- **EF Core Tools** for database migrations

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/todo-app.git
cd todo-app
```

### 2. Configure the Database

Update the `appsettings.json` file with your PostgreSQL database connection string:

```json
"ConnectionStrings": {
  "TodoDatabase": "Host=localhost;Database=your_database_name;Username=your_username;Password=your_password"
}
```

### 3. Configure JWT Settings

Add the JWT settings in `appsettings.json`:

```json
"Jwt": {
  "Key": "your_secret_key_here",
  "Issuer": "TodoAuthApi",
  "Audience": "TodoAuthApiUsers"
}
```

### 4. Run Database Migrations

To set up the database, run the following commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the Application

Start the application using:

```bash
dotnet run
```

The application will start on `https://localhost:5001` (for HTTPS) or `http://localhost:5000` (for HTTP).

## API Endpoints

### Authentication Endpoints

1. **Register a New User** (`POST /api/auth/signup`)

   - Registers a new user with a unique username and email.
   - **Request Body**:

     ```json
     {
       "username": "exampleUser",
       "email": "user@example.com",
       "password": "password123"
     }
     ```

   - **Response**:

     ```json
     {
       "message": "User registered successfully"
     }
     ```

2. **Login a User** (`POST /api/auth/signin`)

   - Authenticates a user and returns a JWT token.
   - **Request Body**:

     ```json
     {
       "username": "exampleUser",
       "password": "password123"
     }
     ```

   - **Response**:

     ```json
     {
       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
     }
     ```

### Todo Management Endpoints

All Todo management endpoints require the user to be authenticated by providing a JWT token in the `Authorization` header as `Bearer <token>`.

1. **Create a New Todo** (`POST /api/todo`)

   - Creates a new Todo item for the authenticated user.
   - **Request Body**:

     ```json
     {
       "title": "Buy groceries",
       "description": "Milk, Bread, Eggs, and Butter",
       "isCompleted": false
     }
     ```

   - **Response**:

     ```json
     {
       "id": 1,
       "title": "Buy groceries",
       "description": "Milk, Bread, Eggs, and Butter",
       "isCompleted": false,
       "userId": 1,
       "createdAt": "2024-09-28T08:30:00Z"
     }
     ```

2. **Get All Todos** (`GET /api/todo`)

   - Retrieves all Todo items for the authenticated user.
   - **Response**:

     ```json
     [
       {
         "id": 1,
         "title": "Buy groceries",
         "description": "Milk, Bread, Eggs, and Butter",
         "isCompleted": false,
         "userId": 1,
         "createdAt": "2024-09-28T08:30:00Z"
       },
       ...
     ]
     ```

3. **Get a Todo by ID** (`GET /api/todo/{id}`)

   - Retrieves a single Todo item by its ID.
   - **Response**:

     ```json
     {
       "id": 1,
       "title": "Buy groceries",
       "description": "Milk, Bread, Eggs, and Butter",
       "isCompleted": false,
       "userId": 1,
       "createdAt": "2024-09-28T08:30:00Z"
     }
     ```

4. **Update a Todo** (`PUT /api/todo/{id}`)

   - Updates an existing Todo item.
   - **Request Body**:

     ```json
     {
       "title": "Buy groceries and fruits",
       "description": "Milk, Bread, Eggs, Butter, Apples, Bananas",
       "isCompleted": true
     }
     ```

   - **Response**:

     ```json
     {
       "id": 1,
       "title": "Buy groceries and fruits",
       "description": "Milk, Bread, Eggs, Butter, Apples, Bananas",
       "isCompleted": true,
       "userId": 1,
       "createdAt": "2024-09-28T08:30:00Z"
     }
     ```

5. **Delete a Todo** (`DELETE /api/todo/{id}`)

   - Deletes a Todo item by its ID.
   - **Response**:

     ```json
     {
       "message": "Todo item deleted successfully"
     }
     ```

## Security Considerations

- **Password Storage**: Passwords are hashed using BCrypt before being stored in the database to ensure that plain-text passwords are never stored.
- **JWT Authentication**: JWT tokens are used to secure all API endpoints, and tokens are signed with a secret key defined in the `appsettings.json` file.
- **Authorization**: Users can only access their own Todo items, and this is enforced through the `userId` field in the Todo entity, which is tied to the authenticated user.

## Validation and Error Handling

- All endpoints perform validation checks on request data.
- Appropriate HTTP status codes and error messages are returned for invalid inputs (e.g., 400 Bad Request for validation errors, 401 Unauthorized for invalid credentials).

## Testing

Unit tests can be added using xUnit and Moq for mocking dependencies. Integration tests can be written using `Microsoft.AspNetCore.Mvc.Testing` for testing the entire API in a real environment.

### Example Test Structure

- **`AuthControllerTests.cs`**: Tests for signup and signin functionality, including validation and JWT token generation.
- **`TodoControllerTests.cs`**: Tests for CRUD operations on the Todo entity.

## Future Improvements

1. **Role-Based Authorization**: Implement roles (e.g., Admin, User) to allow more granular control over endpoints.
2. **Pagination and Filtering**: Add pagination and filtering to the `GET /api/todo` endpoint for better management of large lists.
3. **Enhanced Error Handling**: Implement a global error handling mechanism using middleware to standardize error responses.

## Conclusion

The Todo Application provides a foundational API for managing tasks with a robust authentication and authorization mechanism using JWT tokens. With proper validation, error handling, and secure password storage, it is designed to be a secure and scalable application.

For further questions or contributions, feel free to raise an issue or submit a pull request on the [project's GitHub repository](https://github.com/yourusername/todo-app).
