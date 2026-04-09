# 🌍 Blocked Countries & IP Validation API

A .NET Core Web API for managing blocked countries and validating IP addresses using third-party geolocation services.

---

## 📌 Overview

This project provides a RESTful API that allows:

* Blocking/unblocking countries
* Validating IP addresses and detecting their origin country
* Checking if an IP is blocked
* Logging blocked access attempts
* Temporarily blocking countries with automatic expiration

The application uses **in-memory storage** and integrates with external IP geolocation APIs.

---

## 🚀 Tech Stack

* ASP.NET Core Web API (.NET 8)
* HttpClient (for external API calls)
* In-Memory Data Structures (ConcurrentDictionary)
* Background Services (HostedService)
* Swagger (API Documentation)

---

## ⚙️ Features

### 1. Block a Country

**POST** `/api/countries/block`

* Adds a country to the blocked list
* Prevents duplicates

---

### 2. Delete a Blocked Country

**DELETE** `/api/countries/block/{countryCode}`

* Removes a country from the blocked list
* Returns 404 if not found

---

### 3. Get All Blocked Countries

**GET** `/api/countries/blocked`

Supports:

* Pagination (`page`, `pageSize`)
* Search by country code or name

---

### 4. IP Lookup

**GET** `/api/ip/lookup?ipAddress={ip}`

* Retrieves country details using a third-party API
* Uses caller IP if none provided
* Validates IP format

---

### 5. Check if IP is Blocked

**GET** `/api/ip/check-block`

* Detects caller IP
* Fetches country from external API
* Checks block status
* Logs the attempt

---

### 6. Get Blocked Attempts Logs

**GET** `/api/logs/blocked-attempts`

Returns:

* IP Address
* Timestamp
* Country Code
* Block Status
* User Agent

Supports pagination

---

### 7. Temporarily Block a Country

**POST** `/api/countries/temporal-block`

Request:

```json
{
  "countryCode": "EG",
  "durationMinutes": 120
}
```

* Blocks country for a limited duration (1–1440 minutes)
* Automatically unblocks after expiration
* Prevents duplicate temporary blocks

---

## 🔄 Background Job

A background service runs every 5 minutes to:

* Remove expired temporary blocks automatically

---

## 🌐 External API Integration

This project uses a third-party IP geolocation service such as:

* https://ipapi.co/
* https://ipgeolocation.io/

> API Key should be configured in `appsettings.json`

---

## 🧠 Architecture

The project follows a clean layered architecture:

```
Controllers → Services → Repositories → Models
```

* Controllers: Handle HTTP requests
* Services: Business logic
* Repositories: In-memory data management
* Background Services: Handle scheduled tasks

---

## 📦 In-Memory Storage

* `ConcurrentDictionary` for blocked countries
* `ConcurrentDictionary` for temporary blocks
* `ConcurrentBag` for logs

Ensures thread-safe operations.

---

## 🛡️ Validation & Error Handling

* Validates country codes (ISO format)
* Validates IP addresses
* Handles duplicate entries
* Enforces duration limits for temporary blocks
* Returns proper HTTP status codes

---

## 📘 API Documentation

Swagger is enabled for easy API testing and exploration:

```
https://localhost:{port}/swagger
```

---

## ▶️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/mahmoud-eltablawy62/Dotnet-Developer-Test-Assignment.git
```

### 2. Navigate to the project

```bash
cd Dotnet-Developer-Test-Assignment
```

### 3. Configure API Key

Edit `appsettings.json`:

```json
"GeoLocationApi": {
  "BaseUrl": "https://ipapi.co/",
  "ApiKey": "YOUR_API_KEY"
}
```

### 4. Run the project

```bash
dotnet run
```

---

## 🧪 Testing

You can test endpoints using:

* Swagger UI

---

## 📊 Future Improvements

* Add unit testing
* Add caching layer
* Persist data using a database
* Improve logging (e.g., Serilog)

---

## 👨‍💻 Author

**Mahmoud Eltablawy**

---

## 📩 Submission

This project was developed as part of a technical assessment for a .NET Developer position.

---
