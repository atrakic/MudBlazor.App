PASS="123"
curl -u admin:admin -X POST \
    "http://localhost:9000/api/users/change_password?login=admin&previousPassword=admin&password=$PASS"
