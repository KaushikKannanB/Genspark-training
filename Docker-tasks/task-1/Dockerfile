FROM debian:bullseye

WORKDIR /app

RUN apt-get update &&\
    apt-get install -y curl &&\
    curl -fsSL https://deb.nodesource.com/setup_22.x | bash - &&\
    apt-get install -y nodejs

COPY . .
EXPOSE 3000

CMD ["node","server.js"]