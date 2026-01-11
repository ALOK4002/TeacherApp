#!/bin/bash

echo "Testing Authentication API Endpoints"
echo "===================================="

# Test HTTPS endpoint (7014)
echo ""
echo "1. Testing HTTPS endpoint (https://localhost:7014)"
echo "---------------------------------------------------"

echo "Testing registration on HTTPS..."
curl -X POST https://localhost:7014/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser_https", "password": "password123"}' \
  -k -w "\nHTTP Status: %{http_code}\n" -s

echo ""
echo "Testing login on HTTPS..."
curl -X POST https://localhost:7014/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser_https", "password": "password123"}' \
  -k -w "\nHTTP Status: %{http_code}\n" -s

# Test HTTP endpoint (5162)
echo ""
echo "2. Testing HTTP endpoint (http://localhost:5162)"
echo "------------------------------------------------"

echo "Testing registration on HTTP..."
curl -X POST http://localhost:5162/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser_http", "password": "password123"}' \
  -w "\nHTTP Status: %{http_code}\n" -s

echo ""
echo "Testing login on HTTP..."
curl -X POST http://localhost:5162/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail": "testuser_http", "password": "password123"}' \
  -w "\nHTTP Status: %{http_code}\n" -s

echo ""
echo "Testing complete!"
echo "Expected HTTP Status codes:"
echo "- 200: Success"
echo "- 400: Bad Request (validation errors)"
echo "- 401: Unauthorized (invalid credentials)"
echo "- 500: Internal Server Error"