import { Cardinality, ApiType, SpecType, Exposure, BrokerType, MessageFormat, MessageType, SoftwareComponentType, Language, DatabaseTechnology } from './enums';

export interface Publisher {
  id: string;
  name: string;
}

export interface Broker {
  id: string;
  name: string;
  version: string;
  type: BrokerType;
  clusterName: string;
}

export interface Topic {
  id: string;
  name: string;
  brokerId: string;
  broker?: Broker;
  numberOfPartitions: number;
}

export interface MessageSchema {
  id: string;
  name: string;
  version: string;
  format: MessageFormat;
  type: MessageType;
}

export interface MessageConnection {
  id: string;
  topicId: string;
  topic?: Topic;
  messageSchemaIds: string[];
  messages: MessageSchema[];
}

export interface Runtime {
  id: string;
  name: string;
  version: string;
  vendor: string;
  lts: boolean;
  eol?: string;
}

export interface Database {
  id: string;
  name: string;
  version: string;
  technology: DatabaseTechnology;
}

export interface SoftwareComponent {
  id: string;
  name: string;
  repoUrl: string;
  publisherId?: string;
  publishedBy?: Publisher;
  type: SoftwareComponentType;
  version: string;
  language: Language;
  componentIds: string[];
  components: SoftwareComponent[];
}

export interface ApiDefinition {
  id: string;
  name: string;
  version: string;
  type: ApiType;
  serviceUrl: string;
  specUrl: string;
  specType: SpecType;
  exposure: Exposure;
}

export interface Connection {
  id: string;
  name: string;
  cardinality: Cardinality;
  fromId: string;
  toId: string;
}

export interface Workload {
  id: string;
  name: string;
  repoUrl: string;
  tag: string;
  softwareComponentIds: string[];
  softwareComponents: SoftwareComponent[];
  runtimeId?: string;
  runtime?: Runtime;
  apisExposedIds: string[];
  apisExposed: ApiDefinition[];
  apisInvokedIds: string[];
  apisInvoked: ApiDefinition[];
  consumeMessageFromIds: string[];
  consumeMessageFrom: MessageConnection[];
  produceMessageToIds: string[];
  produceMessageTo: MessageConnection[];
  databaseIds: string[];
  databases: Database[];
}

