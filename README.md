# NestJS-Microservices-Docker-RabbitMQ

## Scripts folder

In some folder after the root we can see a folder called `scripts`. Inside that folder we have lot of `.sh` scripts.

To be able to run those commands we should do the following: 

```bash
cd scripts
chmod a+x filename.sh
```

If you want to run those commands you **MUST** be inside scripts folder.

## Requirements

- Docker
- RabbitMQ (use the command: `docker run -d --network docker-test-1 --hostname rabbitmq -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management`
- NestJS

## Usage with docker-compose and environment handling

To handle different environment we provide the base `docker-compose.yml` file plus 3 environment overrides (local, development and production).

The command to make everything work is 1 and it's very simple: 

```bash
docker compose -f docker-compose.yml -f docker-compose.development.yml up -d --build
```

You can change the `.development` in any other string available (in our case with `.local` or `.production`).

The image rebuilds every time you run this command thanks to the `--build` flag

