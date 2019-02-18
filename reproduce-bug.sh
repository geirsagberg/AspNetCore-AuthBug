 # Produces 200 OK
curl -X GET "https://localhost:5001/Value" -H "accept: text/plain" -H "Authorization: ApiKey e33f1f66-f2d9-4726-92df-307134e1bd1b" -k -I

 # Produces 200 OK
curl -X GET "https://localhost:5001/OtherValue" -H "accept: text/plain" -k -I

# Produces 401 Unauthorized
curl -X GET "https://localhost:5001/Value" -H "accept: text/plain" -H "Authorization: ApiKey e33f1f66-f2d9-4726-92df-307134e1bd1b" -k -I
