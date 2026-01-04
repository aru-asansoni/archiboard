export enum Cardinality {
  OneToOne = 0,
  OneToMany = 1,
  ManyToOne = 2,
  ZeroToOne = 3,
  OneToZero = 4
}

export enum ApiType {
  REST = 0,
  SOAP = 1,
  GRPC = 2,
  GraphQL = 3,
  NotDefined = 4
}

export enum SpecType {
  OpenAPI = 0,
  WSDL = 1
}

export enum Exposure {
  Public = 0,
  Private = 1
}

export enum BrokerType {
  Kafka = 0,
  RabbitMQ = 1
}

export enum MessageFormat {
  Avro = 0,
  ProtoBuf = 1,
  JSON = 2
}

export enum MessageType {
  Event = 0,
  Command = 1
}

export enum SoftwareComponentType {
  Library = 0,
  Framework = 1
}

export enum Language {
  CSharp = 0,
  Java = 1,
  Typescript = 2,
  Javascript = 3,
  Python = 4,
  GoLang = 5
}

export enum DatabaseTechnology {
  MySQL = 0,
  PostgreSQL = 1,
  SQLServer = 2,
  MongoDB = 3,
  SQLite = 4,
  Neo4J = 5
}

