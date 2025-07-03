# 🏢 Hall Management System

A web-based Hall Management System designed to manage student accommodation, room allotments, payments, complaints, and admin functionalities efficiently. Built with modern technologies to streamline hall administration tasks and improve student experience.

---

## 🚀 Features

- 🛏️ Room allotment and availability tracking
- 👨‍🎓 Student registration and profile management
- 💳 Payment tracking and history
- 🛠️ Complaint management system
- 🧑‍💻 Admin an DSW dashboard for complete control
- 📊 Reports and analytics

---

## 🛠️ Tech Stack

- **Backend:** .NET
- **Frontend:** React.js, Typescript (Can be found in my repository named "Hall_Management_System_Frontend")
- **Database:** SQL Server
- **Authentication:** JWT
- **Other Tools:** Git, GitHub, Postman, Swagger.

---

## 📦 Installation

```bash
# Clone the repository

# Navigate to the project directory

# Fill Connection String in appsettings.json file in server folder (Use SSMS or Azure Data Studio to create the database. No need for explicitly create tables)

# Fill Password Key and Token Key in appsettings.json file

# Fill SMTP settings in appsettings.json file for email notifications

# Run the backend server using Visual Studio or command line (dotnet run)

# Navigate to the frontend directory (If using the frontend repository)

# Install dependencies (npm install)

# Run the frontend application (npm runn dev)

# Modify program.cs file for CORS policy if needed

# DSW admin must needed for full functionality including creation off Hall Admin. Hit /Registration/AddDSW endpoint in swagger/postman to create DSW admin

```
