# housing-repairs-online-api

## Local Development
1. `mv sample.env .env`
2. Set values in `.env`
3. `make run`

## Authentication
Requests to the API require authentication.
The API implements JSON Web Tokens (JWT) for authentication.

A unique, secret identifier is required to generate a JWT.
This should be set in an `AUTHENTICATION_IDENTIFIER` environment variable which will be consumed during startup.

A JWT can be generated using a POST request to the `Authentication` endpoint, i.e.
```http request
POST https://localhost:5001/Authentication?identifier=<AUTHENTICATION_IDENTIFIER>
```
The body of the response will contain a JWT which will expire after 1 minute.

All other requests require a valid JWT to be sent in the `Authorization` header with a value of
`Bearer <JWT TOKEN>`, i.e.
```http request
GET https://localhost:5001/Addresses?postcode=1
Authorization: Bearer <JWT TOKEN>
```
