# TC-Zadanie1
## How to build using Docker

To build an image for docker you need:
- clone the repository
- move 'Dockerfile' up a level: `[ solution folder ] $ mv Dockerfile ..`
- build docker image: `DOCKER_BUILDKIT=1 docker build -f DockerfileZad1 -t local/akovalyovzad1 .`

To run an image using docker: `docker run -d --name akovalyovzad1-container -p 8080:4000 local/akovalyovzad1`
