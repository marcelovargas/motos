Motos
=====

Tecnologias usadas:
- .net 9
- Postgres.
- Kafka
- Docker

Estrutura:
- Pattern Respository + services.

Como correr a webapi:

- cd motos
- sudo docker compose -f docker-compose.yml up -d
- Isso vai criar a BD e atualizar as tabelas e subir todos  os serviços necessarios.
- agora, pode acessar a: http://localhost:5000/swagger/index.html 


