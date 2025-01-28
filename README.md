# User Management API

This is a **User Management API** built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**, with authentication and authorization using **JWT**. The application is containerized using **Docker** and provides RESTful APIs for managing users and roles.

---

## **Features**

- User and Role Management (CRUD operations).
- Secure endpoints using JWT authentication.
- SQLite as the database with auto-migrations.
- Integrated Swagger for API documentation.
- Fully containerized with Docker.
- Validations implemented using DataAnnotations.
- Role deletion is restricted if assigned to users.

---

## **Prerequisites**

Make sure you have the following installed on your system:
1. **.NET SDK 6.0 or later** - [Download](https://dotnet.microsoft.com/download)
2. **Docker** - [Download](https://www.docker.com/products/docker-desktop)

---

## **Setup and Run**

### **1. Clone the Repository**
```bash
git clone https://github.com/kamalofficial007/EmployeeManagement.git
cd <repository-folder>
```

### **2. Update Configuration**
Update the appsettings.json file if needed:
```
{
  "JwtSettings": {
    "Key": "YourSecretKeyHere1234567890",
    "Issuer": "UserManagementAPI",
    "Audience": "UserManagementAPI",
    "TokenLifetimeMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=/app/data/user_management_db.db"
  }
}

```

* Key: Replace with a strong secret key for JWT.
* ConnectionStrings: Ensure the database path points to the appropriate location for Docker.


### **3.##Build and Run the Application Locally
dotnet restore
dotnet build
dotnet run

Docker Setup
Step 1: Build Docker Image
Build the Docker image using the provided Dockerfile:

bash
Copy
Edit
docker build -t user-management-api .
Step 2: Run the Docker Container
Run the container:

bash
Copy
Edit
docker run -d -p 8080:8080 --name user-management-api user-management-api
The application will be accessible at http://localhost:8080.
Step 3: Database Path in Docker
Ensure that the database file is created and accessible within the container. The default database path is /app/data/user_management_db.db.


### **4.##Using JWT Authentication
1. Generate JWT Token
To generate a token, use the /api/auth/login endpoint.

Request:
http
Copy
Edit
POST /api/auth/login HTTP/1.1
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}
Response:
json
Copy
Edit
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
Copy the value of the token field.

2. Use JWT Token to Call Secured Endpoints
You need to include the token in the Authorization header of your requests.

Example Header:
http
Copy
Edit
Authorization: Bearer <your-token>
Example: Get All Users
http
Copy
Edit
GET /api/user HTTP/1.1
Host: localhost:8080
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Response:
json
Copy
Edit
[
  {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "roleId": 1
  }
]
3. Authorize in Swagger
Open Swagger at http://localhost:8080/swagger.
Click the Authorize button in the top-right corner.
Enter the token in this format:
php
Copy
Edit
Bearer <your-token>
Click Authorize. Now you can call secured endpoints directly from Swagger.
API Documentation
Authentication
Method	Endpoint	Description	Auth Required
POST	/api/auth/login	Authenticate and get JWT	No
Users
Method	Endpoint	Description	Auth Required
GET	/api/user	Get all users	Yes
GET	/api/user/{id}	Get a user by ID	Yes
POST	/api/user	Create a new user	Yes
PUT	/api/user/{id}	Update a user	Yes
DELETE	/api/user/{id}	Delete a user	Yes
Roles
Method	Endpoint	Description	Auth Required
GET	/api/role	Get all roles	Yes
GET	/api/role/{id}	Get a role by ID	Yes
POST	/api/role	Create a new role	Yes
DELETE	/api/role/{id}	Delete a role (checks if assigned to users)	Yes
Database
SQLite
The application uses SQLite as the database. The database file will be created in the data folder inside the Docker container.

To access the database:

Execute into the running container:
bash
Copy
Edit
docker exec -it user-management-api sh
Use the sqlite3 command to query the database:
bash
Copy
Edit
sqlite3 /app/data/user_management_db.db
Testing
Use Postman or Swagger for testing endpoints.
To test authentication:
First, log in to obtain a JWT token.
Add the token as a Bearer token in the request headers.
Docker Cleanup
If you want to stop and remove the container:

bash
Copy
Edit
docker stop user-management-api
docker rm user-management-api
If you want to remove the image:

bash
Copy
Edit
docker rmi user-management-api
Future Enhancements
Role-based access control for more granular permissions.
Integration with a relational database like PostgreSQL or SQL Server.
Add unit tests for service and controller layers.


