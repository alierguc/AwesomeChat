# AwesomeChat - Chat Application
## - Project Details
It is a messaging project where you can manage message rooms and users.

## - Project Architecure

While designing the project architecture, I aimed to keep the project management requirements, flexibility and developability at a high level. I designed the **"Onion Architecture"** structure as I had constructed it, provided that the structure was not damaged. This is how the modules are related to each other, as in the image below.
<p align="center">
  <img width="400" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/onion.png?raw=true">
</p>

## - Technologies and extensions used in the project
- #### Monitoring
  - HealthCheck
    - MongoDB HealthCheck
    - RabbitMQ HealthCheck
    - MSSQL HealthCheck
    - Elastic Search HealthCheck
    - Seq HealthCheck
  - Elastic Search
  - Seq
  - Kibana
  - Apm Server
  - Metric Beat
  - File Beat
  - Heart Beat
- #### Database
  - MSSQL
  - MongoDB
- #### Logging
  - Seq
  - Serilog
  - MongoDB Logging
  - MSSQL Logging
  - Local Logging
- #### Communications
  - SignalR
- #### Message Broker
  - RabbitMQ
- ### Orm
  - Entity Framework Core
- ### Other Extensions
  - Identity
  - FluentValidation
  - AutoMapper
  - ApiVersioning
  - MediatR
  
  ## - Monitoring  
<br/>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/metrics.PNG?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/filebeat.PNG?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/heartbeat.PNG?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/apm-kibana.png?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/metrics_explorer.png?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/observability.PNG?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/observability.PNG?raw=true">
</p>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/seq.PNG?raw=true">
</p>

- ***You can use the below docker-compose file for configuration.***


```yaml
version: '2.2'
services:
  apm-server:
    image: docker.elastic.co/apm/apm-server:7.14.1
    depends_on:
      elasticsearch:
        condition: service_healthy
      kibana:
        condition: service_healthy
    cap_add: ["CHOWN", "DAC_OVERRIDE", "SETGID", "SETUID"]
    cap_drop: ["ALL"]
    ports:
    - 8200:8200
    networks:
    - elastic
    command: >
       apm-server -e
         -E apm-server.rum.enabled=true
         -E setup.kibana.host=kibana:5601
         -E setup.template.settings.index.number_of_replicas=0
         -E apm-server.kibana.enabled=true
         -E apm-server.kibana.host=kibana:5601
         -E output.elasticsearch.hosts=["elasticsearch:9200"]
    healthcheck:
      interval: 10s
      retries: 12
      test: curl --write-out 'HTTP %{http_code}' --fail --silent --output /dev/null http://localhost:8200/

  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:7.14.1
    environment:
    - bootstrap.memory_lock=true
    - cluster.name=docker-cluster
    - cluster.routing.allocation.disk.threshold_enabled=false
    - discovery.type=single-node
    - ES_JAVA_OPTS=-XX:UseAVX=2 -Xms1g -Xmx1g
    ulimits:
      memlock:
        hard: -1
        soft: -1
    volumes:
    - esdata:/usr/share/elasticsearch/data
    ports:
    - 9200:9200
    networks:
    - elastic
    healthcheck:
      interval: 20s
      retries: 10
      test: curl -s http://localhost:9200/_cluster/health | grep -vq '"status":"red"'

  kibana:
    image: docker.elastic.co/kibana/kibana:7.14.1
    depends_on:
      elasticsearch:
        condition: service_healthy
    environment:
      ELASTICSEARCH_URL: http://elasticsearch:9200
      ELASTICSEARCH_HOSTS: http://elasticsearch:9200
    ports:
    - 5601:5601
    networks:
    - elastic
    healthcheck:
      interval: 10s
      retries: 20
      test: curl --write-out 'HTTP %{http_code}' --fail --silent --output /dev/null http://localhost:5601/api/status
      
  metricbeat:
    image: docker.elastic.co/beats/metricbeat:7.11.1
    environment:
      ELASTICSEARCH_HOSTS: http://elasticsearch:9200
    networks:
      - elastic
    depends_on:
      elasticsearch:
        condition: service_healthy
        
        
  heartbeat:
    container_name: heartbeat
    hostname: heartbeat
    user: root #To read the docker socket
    image: "docker.elastic.co/beats/heartbeat:7.17.8"
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    depends_on:
      elasticsearch: { condition: service_healthy }
    command: heartbeat -e -strict.perms=false
    networks:
      - elastic
    restart: on-failure
  
  
  auditbeat:
    container_name: auditbeat
    hostname: auditbeat
    user: root #To read the docker socket
    image: "docker.elastic.co/beats/auditbeat:7.17.8"
    depends_on:
      elasticsearch: { condition: service_healthy }
    command: auditbeat -e -strict.perms=false
    pid: "host" # Set the required permissions with pid and cap_add for Auditbeat
    cap_add: ['AUDIT_CONTROL', 'AUDIT_READ']
    networks:
      - elastic
    restart: on-failure
  
        
  filebeat:
    image: docker.elastic.co/beats/filebeat:${ELASTIC_VERSION:-7.9.1}
    # https://github.com/docker/swarmkit/issues/1951
    hostname: "{{.Node.Hostname}}-filebeat"
    # Need to override user so we can access the log files, and docker.sock
    user: root
    networks:
      - elastic
    environment:
      - ELASTICSEARCH_HOST=http://elasticsearch:9200
      - KIBANA_HOST=http://elasticsearch:5601
    # disable strict permission checks
    command: ["--strict.perms=false"]
    deploy:
      mode: global  


volumes:
  esdata:
    driver: local

networks:
  elastic:
    driver: bridge
```

