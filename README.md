#Redis Singleton pattern framework with StackExchange.Redis

##Features

- Connection Mapping with Configuration
- Configuration with Redis Instance Group and Name concept supported
- Singleton pattern avoid resource waste

##Dependency

- StackExchange.Redis
- FX.Configuration
- Newtonsoft.Json
- Log4Net(Optional)

##Setup with Dockerizing a Redis service

1. Enable Virtualization Technology on Bios and install Docker Toolbox
2. Create a Docker container for Redis
3. Run the service
4. Create your web application container

Detail Reference of [Dockerizing a Redis service](https://docs.docker.com/examples/running_redis_service/)

####*If any problem you can remove and setup again*

    docker-machine rm default
    docker-machine --native-ssh create -d virtualbox default

##How to Use

###
ConnectionSetting follow StackExchange.Redis
ConfigurationOptions.Parse()

### Example wtih Unit Test
- Example – Basic 
	- Strings
	- Lists
	- Sets
	- Hashes
	- Sorted Sets
- Example – Advance
	- Sort
	- Expire
	- HashSet, HashGetAll and HashDelete
	- Pub/Sub
	- Pipelines
	- Batch
- Example – Scripting

#Redis Tutoring
