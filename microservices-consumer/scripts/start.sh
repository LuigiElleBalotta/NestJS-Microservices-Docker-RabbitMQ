 #!/bin/sh
docker run -dp 3006:3006 --name microservices-consumer --network docker-test-1 microservices-consumer
