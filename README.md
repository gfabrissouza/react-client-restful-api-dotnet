# RESTful API with .NET, Docker and React

This project demonstrates how to build a professional-grade RESTful API from scratch using ASP.NET Core, with a focus on security, scalability, and real-world practices. It includes integration with a React frontend and deployment using Docker and GitHub Actions.

## 📌 Key Features

- ✅ Fully RESTful API with resource-based routes
- ✅ JWT authentication with access and refresh tokens via **HttpOnly Cookies**
- ✅ Secure OAuth login (e.g., Google) using Authorization Code
- ✅ Versioning, Pagination, File Upload & Download support
- ✅ **HATEOAS** implementation to enrich responses
- ✅ Swagger (OpenAPI) documentation
- ✅ Clean Architecture with Business, Repository, and Controller layers
- ✅ Docker and Docker Compose for local development
- ✅ Secure HTTPS with NGINX reverse proxy
- ✅ Continuous Integration and Deployment using GitHub Actions

---

## 🔐 Security Architecture

| Component                  | Description                                                                 |
|---------------------------|-----------------------------------------------------------------------------|
| Access Token              | Stored in HttpOnly, Secure Cookie with short expiration (~15 min)          |
| Refresh Token             | Stored in HttpOnly, Secure Cookie with long expiration (~7 days)           |
| Authentication Flow       | OAuth 2.0 Authorization                                                    |
| Token Refresh             | Done via `/refresh` endpoint using cookie, without exposing new token      |
| Session Info              | Retrieved via `/me` endpoint (protected by `Authorize`)                    |
| CORS                      | Configured to allow cross-origin requests with `withCredentials`           |
| Secure Cookies            | Sent only over HTTPS with `SameSite=None` for cross-domain compatibility   |
| Token Reading             | Tokens are read from cookies (not headers) via `OnMessageReceived` hook    |
| Logout Flow               | Deletes access and refresh token cookies                                   |

✅ **Protected Against**:
- XSS (Cross-Site Scripting)
- CSRF (Cross-Site Request Forgery)
- Token Theft / Replay Attacks
- Unauthorized API access

---

## 🧱 Architecture Layers

```
src/
├── Business/              # Business logic
├── Repository/            # Data access (EF Core + GenericRepository)
├── Model/                 # Entities and DTOs
├── Configurations/        # Token, CORS, Swagger configs
├── Controllers/           # API Controllers
├── Services/              # OAuth, Token management
├── Hypermedia/            # HATEOAS Enrichers
└── Program.cs             # App startup and DI setup
```

---

## 🧪 REST Concepts and Best Practices

| Concept                     | Applied in Project                             |
|----------------------------|-------------------------------------------------|
| REST Maturity Model        | Level 3 (with HATEOAS and content negotiation)  |
| REST Verbs                 | GET, POST, PUT, PATCH, DELETE                   |
| URL Parameters             | Query, Path, Header, Body                       |
| API Versioning             | URL versioning (`v1`, `v2`)                     |
| Pagination                 | Via query params (page, limit)                 |
| Media Types                | Supports `application/json`, `application/xml` |
| Content Negotiation        | Based on `Accept` headers                      |
| File Download / Upload     | Endpoints for handling file transfers          |

---

## 🧰 Tools & Technologies

- **ASP.NET Core** 8.0
- **Evolve** + MySQL
- **JWT** + OAuth 2.0 (Google)
- **Swagger / OpenAPI**
- **Docker / Docker Compose**
- **NGINX Reverse Proxy**
- **GitHub Actions** for CI/CD
- **React.js** frontend using Axios + Secure Auth

---

## 🔗 Third-Party Integrations

- ✅ Google OAuth 2.0 for user authentication
- ✅ Token revocation and refresh handled securely
- ✅ Fully integrated with React frontend

---

## 🧪 Postman Usage

- Included Postman collection to test API endpoints
- Supports:
  - GET, POST, PUT, DELETE
  - Authenticated requests with cookies
  - File uploads and downloads
  - Pagination and query testing

---

## 🧠 Concepts Learned

- REST theory and HTTP standards
- Security best practices for APIs
- Clean code with separation of concerns
- Building scalable, cloud-ready apps
- Secure authentication without exposing tokens to frontend
- CI/CD automation and cloud deployment

---

## 🖥️ How to Run Locally (Dev)

```bash
# Clone the repository
git clone https://github.com/gfabrissouza/react-client-restful-api-dotnet
cd react-client-restful-api-dotnet

# Launch Docker containers
docker-compose up --build
```

- React frontend runs on: `http://localhost:5173`
- Backend API runs on: `https://localhost:443`
- Swagger UI: `https://localhost/swagger`
- Swagger UI (Docker): `http://localhost:81/swagger`
---

## 🧩 Frontend React Integration

- Uses `withCredentials: true` in Axios to include cookies
- Calls `/me` to get session data
- Automatically refreshes tokens using silent requests
- Fully cross-domain compatible with secure CORS config

---

## 📝 License

This project is licensed under the MIT License.

---

## 👤 Author

**Guilherme Fabris**  
[GitHub](https://github.com/gfabrissouza)
