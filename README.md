# NestJS-Microservices-Docker-RabbitMQ

## Scripts folder

In some folder after the root we can see a folder called `scripts`. Inside that folder we have lot of `.sh` scripts.

To be able to run those commands we should do the following: 

```bash
cd scripts
chmod a+x filename.sh
```

If you want to run those commands you **MUST** be inside scripts folder.

## Requirements

- Docker
- RabbitMQ (use the command: `docker run -d --network docker-test-1 --hostname rabbitmq -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management`
- NestJS

