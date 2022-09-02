# NestJS-Microservices-Docker-RabbitMQ

## Requirements

- Docker
- RabbitMQ (use the command: `docker run -d --network docker-test-1 --hostname rabbitmq -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management`
- NestJS

## Local development without docker-compose

### Scripts folder

In some folder after the root we can see a folder called `scripts`. Inside that folder we have lot of `.sh` scripts.

To be able to run those commands we should do the following: 

```bash
cd scripts
chmod a+x filename.sh
```

If you want to run those commands you **MUST** be inside scripts folder.

We suggest to **rebuild** the image everytime, so use the `rebuild_and_start.sh` script.

## Usage with docker-compose and environment handling

To handle different environment we provide the base `docker-compose.yml` file plus 3 environment overrides (local, development and production).

The command to make everything work is 1 and it's very simple: 

```bash
docker compose -f docker-compose.yml -f docker-compose.local.yml --project-name "project_name_all_lowercase" up -d --build
```

You can change the `.local` in any other string available (in our case with `.development` or `.production`).

The image rebuilds every time you run this command thanks to the `--build` flag

Note that the `.local.yml` file have the `build` field and `image` field specified. The image will be built using the local source files.

Inside the `.development.yml` and `.production.yml` we will have only the `image` field that will specify the image name and tag from DockerHub.

By doing this technique we can avoid having the source code in our production or test servers. By the way, we will always need files like `.env.*` files in the server, because without that file the software inside the container won't have any environment variable available.



