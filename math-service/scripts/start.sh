 #!/bin/sh
docker run -dp 3007:3007 --name math-service --network docker-test-1 math-service
