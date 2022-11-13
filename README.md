# DockerGettingStarted
https://github.com/JoonaKleemol/DockerGettingStarted

To get started run following commands in (for example) Powershell. Note the <fileserver-name> parameter for running the applications (examples below)


Build the images with 

```properties
docker compose build
```

Create new network 

```properties
docker network create DockerAppNetwork
```

Run server application

```properties
docker run -d --network DockerAppNetwork --name <fileserver-name> --mount source=servervol,target=/serverdata serverapp
```

And after that run the client application

```properties
docker run -d --network DockerAppNetwork --name clientserver.local --mount source=clientvol,target=/clientdata clientapp "<fileserver-name>:80"
```

For example 


```properties
docker run -d --network DockerAppNetwork --name fileserver.local --mount source=servervol,target=/serverdata serverapp
```

```properties
docker run -d --network DockerAppNetwork --name clientserver.local --mount source=clientvol,target=/clientdata clientapp "fileserver.local:80"
```


