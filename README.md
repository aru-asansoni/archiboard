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

graph TD

%% Message broker and topic
topicA[topic-a]
kafka[kafka]
messageBroker[message-broker]
kafkaVersion["version = 1234"]

topicA -- serves --> kafka
kafka -- is-served-by --> topicA

kafka -- is-a --> messageBroker
kafka -- has-a --> kafkaVersion

%% Application consuming topic
app001[app-001]
topicA -- is-consumed-by --> app001
app001 -- consumes --> topicA

%% Kubernetes hosting app
k8s001[k8s-001]
app001 -- is-hosted-on --> k8s001
k8s001 -- hosts --> app001

%% Kafka hosting on VM
vm001a[vm-001]
kafka -- is-hosted-on --> vm001a
vm001a -- hosts --> kafka

%% Virtual machines and hypervisor
virtualMachine[virtual-machine]
hypervisor[hypervisor]
hypervisorKind["kind = hyper-v"]
hypervisorVersion["version = 1234"]

vm001a -- is-a --> virtualMachine
virtualMachine -- uses --> hypervisor
hypervisor -- is-used-by --> virtualMachine

hypervisor -- has-a --> hypervisorKind
hypervisorKind -- has-a --> hypervisorVersion

%% Cluster composition
cluster001[cluster-001]
vm001b[vm-001]
vm001c[vm-001]

cluster001 -- comprises --> vm001b
vm001b -- is-comprised-of --> cluster001

cluster001 -- comprises --> vm001c
vm001c -- is-comprised-of --> cluster001

%% Kubernetes hosted on cluster
k8s001 -- is-hosted-on --> cluster001
cluster001 -- hosts --> k8s001
