# archiboard
Architecture configuration manager

The purpose of Archiboard application is to collect the main characteristics of an architecture, enumerating and detailing the properties of all workloads and the connection between them.
Every component of a software architecture must be comprised, as for example, a GUI, an API Gateway, Web APIs, Message Broker and a Database.

Structure of the project:
"frontend": GUI developed in Angular 21
"backend": Web API developed in ASP.NET Core 10
"tools": several tools to help build and run the application. Can contain Microcks artifacts, Database seedeer and Docker compose file

e2e tests are implemented with Playwright

Follow some configuration items that capture a possible architecture:

# Configuration Items

---

## Connection
- **Name**
- **Cardinality**: Enum  
  - `1..1`  
  - `1..*`  
  - `*..1`  
  - `0..1`  
  - `1..0`
- **From** 
- **To**

---

## API
- **Name**
- **Version**: *(SemVer)*
- **Type**: Enum  
  - REST  
  - SOAP  
  - gRPC  
  - GraphQL  
  - NotDefined
- **Service Url**
- **Spec Url**
- **Spec Type**:  
  - OpenAPI  
  - WSDL
- **Exposure**: Enum  
  - Public  
  - Private

---

## Workload
- **Name**
- **Repo Url**
- **Tag**
- **SoftwareComponents**: `SoftwareComponent[]`
- **Runtime**: `Runtime`
- **APIsExposed**: `API[]`
- **APIsInvoked**: `API[]`
- **ConsumeMessageFrom**: `MessageConnection[]`
- **ProduceMessageTo**: `MessageConnection[]`
- **Databases**: `Database[]`

---

## MessageConnection
- **Topic**: `Topic`
- **Messages**: `MessageSchema[]`

---

## Topic
- **Name**
- **Broker**: `Broker`
- **NumberOfPartitions**

---

## Broker
- **Name**
- **Version**
- **Type**: Enum  
  - Kafka  
  - RabbitMQ
- **ClusterName**

---

## MessageSchema
- **Name**
- **Version**
- **Format**: Enum  
  - Avro  
  - ProtoBuf  
  - JSON
- **Type**:  
  - Event  
  - Command

---

## SoftwareComponent
- **Name**
- **RepoUrl**
- **PublishedBy**: `Publisher`
- **Type**: Enum  
  - Library  
  - Framework
- **Version**
- **Language**: Enum  
  - C#  
  - Java  
  - Typescript  
  - Javascript  
  - Python  
  - GoLang
- **Components**: `SoftwareComponent[]`

---

## Runtime
- **Name**
- **Version**
- **Vendor**
- **LTS**:  
  - True  
  - False
- **EOL**: `DateTime`

---

## Database
- **Name**
- **Version**
- **Technology**: Enum  
  - MySQL  
  - PostgreSQL  
  - SQL Server  
  - MongoDB  
  - SQLite  
  - Neo4J

