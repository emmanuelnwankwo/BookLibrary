# Book Library API


## Description
The Book Library API provides local library automate the process of managing book reservations and borrowing for customers. The API allows customers to search for available books, reserve them for collection, and borrow them online. It also ensures that books already reserved or borrowed are unavailable for other customers during the reservation period.

## Key Features
- Book Search: Customers can search for books in the library's collection by title.
- Book Reservation: Customers can reserve books for a 24-hour period if available. Reserved books cannot be borrowed or reserved by others during this period.
- Book Borrowing: Customers can borrow available books. If a book is already borrowed, the API informs the customer when it will be returned.
- Authentication & Authorization: Access to customer and book data is restricted to authenticated users.
- API Documentation: The API is well-documented, allowing front-end engineers to easily integrate it with any user-facing platform.
- Notification System: Customers can request notifications for books that are currently borrowed or reserved. The API stores the notification request and notifies the customer when the book becomes available.

## Technology Stack
- Backend Framework: .NET Core 8
- Database: PostgreSQL for storing book and customer data
- Authentication: JWT-based authentication for securing API access
- Documentation: Swagger/OpenAPI for API documentation
- Notification System: Database-persisted notifications for book availability

## Documentation
Book Library API is hosted on Render at [Docs](https://booklibrary-1-2twv.onrender.com/swagger/index.html). This documentation provides more detailed information about the API, including how to use it, what data it provides, and any other features or requirements.

## Installation and Setup
### Running the Project Locally
1. Clone the repository
```bash
    git clone https://github.com/your-repo/library-management-api.git
```
2. Open project on your IDE (Visual Studio or Visual Studio Code)
3. Restore the required dependencies
```bash
    dotnet restore
```
4. Update the appsettings.json file with your PostgreSQL connection string and JWT secret.
5. Run the application
```bash
    dotnet run
```
6. Access the API documentation at http://localhost:5480/swagger/index.html to interact with the API.

### Running the Project with Docker
You can also run the project using Docker and docker-compose. Follow these steps:
1. Ensure Docker is installed on your machine.
2. Build and run the containers:
```bash
$ docker-compose up --build
```
3. The API will be available at http://localhost:8080 and PostgreSQL will run within the container as the database.
4. Access the API documentation at http://localhost:8080/swagger/index.html to interact with the API.

## Environment Variables
The following environment variables are used in docker-compose.yml for database connectivity:
```
POSTGRES_DB: <database>
POSTGRES_USER: <postgres>
POSTGRES_PASSWORD: <postgres>.
```

