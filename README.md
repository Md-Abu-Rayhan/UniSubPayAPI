# Multi-Tournament Checkout API
- .NET 9 (TargetFramework)


## Overview

The API provides endpoints for:
- Browsing available tournaments
- Registering for tournaments with one-time payments
- Creating weekly or monthly subscriptions for tournaments
- Processing payment callbacks

## API Endpoints

### Tournament Management

#### Get All Tournaments
```
POST /api/Tournament/all
```
**Request Body**: Empty object `{}`


**Response**:
```json
[
  {
    "id": 1,
    "name": "Summer Chess Tournament",
    "description": "Annual summer chess tournament for all skill levels",
    "startDate": "2024-04-16T15:55:16.120Z",
    "endDate": "2024-04-18T15:55:16.120Z",
    "entryFee": 50.00,
    "maxParticipants": 64,
    "isActive": true,
    "location": "Dhaka Chess Club",
    "organizerId": "admin"
  },
  {
    "id": 2,
    "name": "Weekly Poker Night",
    "description": "Weekly poker tournament with cash prizes",
    "startDate": "2024-03-24T15:55:16.120Z",
    "endDate": "2024-03-24T15:55:16.120Z",
    "entryFee": 100.00,
    "maxParticipants": 10,
    "isActive": true,
    "location": "Card Room, Gulshan",
    "organizerId": "admin"
  }
]
```

#### Get Tournament Details
```
POST /api/Tournament/details
```
**Request Body**:
```json
{
  "tournamentId": 1
}
```

**Response**:
```json
{
  "id": 1,
  "name": "Summer Chess Tournament",
  "description": "Annual summer chess tournament for all skill levels",
  "startDate": "2024-04-16T15:55:16.120Z",
  "endDate": "2024-04-18T15:55:16.120Z",
  "entryFee": 50.00,
  "maxParticipants": 64,
  "isActive": true,
  "location": "Dhaka Chess Club",
  "organizerId": "admin"
}
```

### One-Time Payment Checkout

#### Create Checkout
```
POST /api/Checkout/create
```
**Request Body**:
```json
{
  "userId": 1,
  "tournamentIds": [1, 2],
  "returnUrl": "https://example.com/success",
  "cancelUrl": "https://example.com/cancel"
}
```

**Response**:
```json
{
  "checkoutUrl": "https://bkashtest.shabox.mobi/home/MultiTournamentInBuildCheckoutUrl?transactionId=abc123&amount=150.00&returnUrl=https://example.com/success&cancelUrl=https://example.com/cancel",
  "transactionId": "abc123",
  "totalAmount": 150.00,
  "status": "Pending"
}
```

#### Process Payment Callback
```
POST /api/Checkout/callback
```
**Request Body**:
```json
{
  "transactionId": "abc123",
  "status": "Completed",
  "amount": 150.00,
  "currency": "BDT",
  "paymentMethod": "bKash"
}
```

**Response**: `true` or `false`

### Subscription Management

#### Create Weekly Subscription
```
POST /api/Subscription/create/weekly
```
**Request Body**:
```json
{
  "userId": 1,
  "tournamentId": 1,
  "amount": 1,
  "redirectUrl": "https://example.com/success",
  "subscriptionReference": "01712345678"
}
```

**Response**:
```json
{
  "paymentUrl": "https://bkashtest.shabox.mobi/home/MultiTournamentInBuildCheckoutUrl?subscriptionRequestId=SUB-abc123&amount=1&redirectUrl=https://example.com/success",
  "subscriptionRequestId": "SUB-abc123",
  "status": "Pending",
  "frequency": "WEEKLY"
}
```

#### Create Monthly Subscription
```
POST /api/Subscription/create/monthly
```
**Request Body**:
```json
{
  "userId": 1,
  "tournamentId": 1,
  "amount": 1,
  "redirectUrl": "https://example.com/success",
  "subscriptionReference": "01712345678"
}
```

**Response**:
```json
{
  "paymentUrl": "https://bkashtest.shabox.mobi/home/MultiTournamentInBuildCheckoutUrl?subscriptionRequestId=SUB-def456&amount=1&redirectUrl=https://example.com/success",
  "subscriptionRequestId": "SUB-def456",
  "status": "Pending",
  "frequency": "MONTHLY"
}
```

## Use Cases

### 1. Tournament Registration with One-Time Payment

**Scenario**: A user wants to register for multiple tournaments with a single payment.

**Steps**:
1. Browse available tournaments:
   ```
   POST /api/Tournament/all
   ```

2. Select tournaments and create checkout:
   ```
   POST /api/Checkout/create
   {
     "userId": 1,
     "tournamentIds": [1, 2],
     "returnUrl": "https://example.com/success",
     "cancelUrl": "https://example.com/cancel"
   }
   ```

3. Redirect user to the payment URL received in the response.

4. After payment completion, the payment gateway will call the callback endpoint:
   ```
   POST /api/Checkout/callback
   {
     "transactionId": "abc123",
     "status": "Completed",
     "amount": 150.00,
     "currency": "BDT",
     "paymentMethod": "bKash"
   }
   ```

### 2. Weekly Tournament Subscription

**Scenario**: A user wants to subscribe to a weekly tournament.

**Steps**:
1. Get tournament details:
   ```
   POST /api/Tournament/details
   {
     "tournamentId": 1
   }
   ```

2. Create weekly subscription:
   ```
   POST /api/Subscription/create/weekly
   {
     "userId": 1,
     "tournamentId": 1,
     "amount": 1,
     "redirectUrl": "https://example.com/success",
     "subscriptionReference": "01712345678"
   }
   ```

3. Redirect user to the payment URL received in the response.

### 3. Monthly Tournament Subscription

**Scenario**: A user wants to subscribe to a monthly tournament.

**Steps**:
1. Get tournament details:
   ```
   POST /api/Tournament/details
   {
     "tournamentId": 1
   }
   ```

2. Create monthly subscription:
   ```
   POST /api/Subscription/create/monthly
   {
     "userId": 1,
     "tournamentId": 1,
     "amount": 1,
     "redirectUrl": "https://example.com/success",
     "subscriptionReference": "01712345678"
   }
   ```

3. Redirect user to the payment URL received in the response.

## Testing

You can test the API using:

### Swagger UI
Access the Swagger UI at: http://localhost:5230/swagger/index.html

### Postman
1. Create a new collection for the Multi-Tournament Checkout API
2. Add requests for each endpoint with the appropriate request bodies
3. Test the flow by first getting tournaments, then creating a checkout or subscription


## Error Handling

All endpoints return appropriate HTTP status codes:
- 200: Success
- 400: Bad Request (invalid input)
- 404: Not Found (resource not found)
- 500: Internal Server Error

Error responses include a message explaining the error.