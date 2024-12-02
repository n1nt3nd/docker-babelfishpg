# Babelfish PostgreSQL with Docker and .NET

This repository is a fork of [jonathanpotts/docker-babelfishpg](https://github.com/jonathanpotts/docker-babelfishpg). It includes additional convenience features, such as a `compose.yaml` file for easier setup.

## What is Babelfish?

Babelfish is an extension for PostgreSQL that enables it to understand SQL Server's wire protocol and T-SQL syntax. This allows PostgreSQL to act as a hybrid database, enabling applications written for SQL Server to work with minimal or no changes. It provides a seamless way to migrate SQL Server workloads to PostgreSQL while maintaining compatibility with existing tools like SQL Server Management Studio (SSMS).

In this project, we use Docker to set up a Babelfish-enabled PostgreSQL instance and demonstrate its integration with a .NET C# migration app.

---

## Getting Started

### 1. Build and Start the Containers
1. Build the Docker containers:  
   ```bash
   docker compose build
   ```
2. Start the containers:  
   ```bash
   docker compose up -d
   ```

### 2. Run the .NET Migration App
Navigate to the `ExecuteSqlScript` directory and run the C# console app:  
```bash
dotnet run
```  
This app runs a SQL Server migration against the PostgreSQL container configured with Babelfish.

### Database Credentials
Use the following credentials for connecting to the database:

- **Username:** `babelfish_user`
- **Password:** `12345678`
- **Database:** `babelfish_db`

### Exploring the Database
- **SQL Server Management Studio (SSMS):**  
  Connect to the **master** database to view the changes made by the migration app.
  
- **pgAdmin:**  
  Navigate to the `babelfish_db` database to explore its contents.

---

## Tear Down the Environment
To stop and remove the containers, navigate to the root directory of the repository and run:  
```bash
docker compose down -v
```
