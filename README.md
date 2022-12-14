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
App Metric
<br/>
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/metrics.PNG?raw=true">
</p>
File Beat
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/filebeat.PNG?raw=true">
</p>
Heart Beat
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/heartbeat.PNG?raw=true">
</p>
Apm Kibana
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/apm-kibana.png?raw=true">
</p>
Metrics Explorer
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/metrics_explorer.png?raw=true">
</p>
Observability
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/observability.PNG?raw=true">
</p>
Observability
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/observability.PNG?raw=true">
</p>
Seq
<p align="center">
  <img width="700" height="350" src="https://github.com/alierguc/AwesomeChat/blob/master/docs/seq.PNG?raw=true">
</p>
