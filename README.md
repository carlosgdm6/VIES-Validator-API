# 🌍 VIES Validator API - EU VAT Number Validation

This API allows you to validate European VAT numbers using the official **VIES** (VAT Information Exchange System). It ensures that the VAT number belongs to a registered business within the European Union.

---

## ⚙️ Features

- ✅ Validate EU VAT numbers in real time
- 🛡️ Secure access via token-based authentication
- 🔁 Designed for integration into ERPs, invoicing systems, or compliance tools
- 📊 Clear JSON responses with validation status and company details (if available)
- 🧩 Modular architecture for easy extension

---

## 🔐 Authentication

This API supports role-based access (Admin/User) using access tokens. Each request must include a valid token in the `Authorization` header.

---

## 🚀 Technologies Used

- ASP.NET Core Web API  
- SOAP integration with VIES  
- Token-based authentication  
- Role management (Admin/User)  
- JSON response formatting

---

## 📥 Example Request

```http
GET /api/validacao/{vatNumber}
Authorization: Bearer {your_token}
