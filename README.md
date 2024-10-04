Instructions on how to run keycloak for testing. Note, you'll need to setup some clients/realm. 

docker run -d -p 8888:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:25.0.6 start-dev
