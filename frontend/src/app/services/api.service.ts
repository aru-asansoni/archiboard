import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Publisher, Broker, Topic, MessageSchema, MessageConnection,
  Runtime, Database, SoftwareComponent, ApiDefinition, Connection, Workload
} from '../models/entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7000/api';

  constructor(private http: HttpClient) {}

  // Publishers
  getPublishers(): Observable<Publisher[]> {
    return this.http.get<Publisher[]>(`${this.baseUrl}/publishers`);
  }
  getPublisher(id: string): Observable<Publisher> {
    return this.http.get<Publisher>(`${this.baseUrl}/publishers/${id}`);
  }
  createPublisher(publisher: Publisher): Observable<Publisher> {
    return this.http.post<Publisher>(`${this.baseUrl}/publishers`, publisher);
  }
  updatePublisher(id: string, publisher: Publisher): Observable<Publisher> {
    return this.http.put<Publisher>(`${this.baseUrl}/publishers/${id}`, publisher);
  }
  deletePublisher(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/publishers/${id}`);
  }

  // Brokers
  getBrokers(): Observable<Broker[]> {
    return this.http.get<Broker[]>(`${this.baseUrl}/brokers`);
  }
  getBroker(id: string): Observable<Broker> {
    return this.http.get<Broker>(`${this.baseUrl}/brokers/${id}`);
  }
  createBroker(broker: Broker): Observable<Broker> {
    return this.http.post<Broker>(`${this.baseUrl}/brokers`, broker);
  }
  updateBroker(id: string, broker: Broker): Observable<Broker> {
    return this.http.put<Broker>(`${this.baseUrl}/brokers/${id}`, broker);
  }
  deleteBroker(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/brokers/${id}`);
  }

  // Topics
  getTopics(): Observable<Topic[]> {
    return this.http.get<Topic[]>(`${this.baseUrl}/topics`);
  }
  getTopic(id: string): Observable<Topic> {
    return this.http.get<Topic>(`${this.baseUrl}/topics/${id}`);
  }
  createTopic(topic: Topic): Observable<Topic> {
    return this.http.post<Topic>(`${this.baseUrl}/topics`, topic);
  }
  updateTopic(id: string, topic: Topic): Observable<Topic> {
    return this.http.put<Topic>(`${this.baseUrl}/topics/${id}`, topic);
  }
  deleteTopic(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/topics/${id}`);
  }

  // MessageSchemas
  getMessageSchemas(): Observable<MessageSchema[]> {
    return this.http.get<MessageSchema[]>(`${this.baseUrl}/messageschemas`);
  }
  getMessageSchema(id: string): Observable<MessageSchema> {
    return this.http.get<MessageSchema>(`${this.baseUrl}/messageschemas/${id}`);
  }
  createMessageSchema(schema: MessageSchema): Observable<MessageSchema> {
    return this.http.post<MessageSchema>(`${this.baseUrl}/messageschemas`, schema);
  }
  updateMessageSchema(id: string, schema: MessageSchema): Observable<MessageSchema> {
    return this.http.put<MessageSchema>(`${this.baseUrl}/messageschemas/${id}`, schema);
  }
  deleteMessageSchema(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/messageschemas/${id}`);
  }

  // MessageConnections
  getMessageConnections(): Observable<MessageConnection[]> {
    return this.http.get<MessageConnection[]>(`${this.baseUrl}/messageconnections`);
  }
  getMessageConnection(id: string): Observable<MessageConnection> {
    return this.http.get<MessageConnection>(`${this.baseUrl}/messageconnections/${id}`);
  }
  createMessageConnection(connection: MessageConnection): Observable<MessageConnection> {
    return this.http.post<MessageConnection>(`${this.baseUrl}/messageconnections`, connection);
  }
  updateMessageConnection(id: string, connection: MessageConnection): Observable<MessageConnection> {
    return this.http.put<MessageConnection>(`${this.baseUrl}/messageconnections/${id}`, connection);
  }
  deleteMessageConnection(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/messageconnections/${id}`);
  }

  // Runtimes
  getRuntimes(): Observable<Runtime[]> {
    return this.http.get<Runtime[]>(`${this.baseUrl}/runtimes`);
  }
  getRuntime(id: string): Observable<Runtime> {
    return this.http.get<Runtime>(`${this.baseUrl}/runtimes/${id}`);
  }
  createRuntime(runtime: Runtime): Observable<Runtime> {
    return this.http.post<Runtime>(`${this.baseUrl}/runtimes`, runtime);
  }
  updateRuntime(id: string, runtime: Runtime): Observable<Runtime> {
    return this.http.put<Runtime>(`${this.baseUrl}/runtimes/${id}`, runtime);
  }
  deleteRuntime(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/runtimes/${id}`);
  }

  // Databases
  getDatabases(): Observable<Database[]> {
    return this.http.get<Database[]>(`${this.baseUrl}/databases`);
  }
  getDatabase(id: string): Observable<Database> {
    return this.http.get<Database>(`${this.baseUrl}/databases/${id}`);
  }
  createDatabase(database: Database): Observable<Database> {
    return this.http.post<Database>(`${this.baseUrl}/databases`, database);
  }
  updateDatabase(id: string, database: Database): Observable<Database> {
    return this.http.put<Database>(`${this.baseUrl}/databases/${id}`, database);
  }
  deleteDatabase(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/databases/${id}`);
  }

  // SoftwareComponents
  getSoftwareComponents(): Observable<SoftwareComponent[]> {
    return this.http.get<SoftwareComponent[]>(`${this.baseUrl}/softwarecomponents`);
  }
  getSoftwareComponent(id: string): Observable<SoftwareComponent> {
    return this.http.get<SoftwareComponent>(`${this.baseUrl}/softwarecomponents/${id}`);
  }
  createSoftwareComponent(component: SoftwareComponent): Observable<SoftwareComponent> {
    return this.http.post<SoftwareComponent>(`${this.baseUrl}/softwarecomponents`, component);
  }
  updateSoftwareComponent(id: string, component: SoftwareComponent): Observable<SoftwareComponent> {
    return this.http.put<SoftwareComponent>(`${this.baseUrl}/softwarecomponents/${id}`, component);
  }
  deleteSoftwareComponent(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/softwarecomponents/${id}`);
  }

  // Apis
  getApis(): Observable<ApiDefinition[]> {
    return this.http.get<ApiDefinition[]>(`${this.baseUrl}/apis`);
  }
  getApi(id: string): Observable<ApiDefinition> {
    return this.http.get<ApiDefinition>(`${this.baseUrl}/apis/${id}`);
  }
  createApi(api: ApiDefinition): Observable<ApiDefinition> {
    return this.http.post<ApiDefinition>(`${this.baseUrl}/apis`, api);
  }
  updateApi(id: string, api: ApiDefinition): Observable<ApiDefinition> {
    return this.http.put<ApiDefinition>(`${this.baseUrl}/apis/${id}`, api);
  }
  deleteApi(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/apis/${id}`);
  }

  // Connections
  getConnections(): Observable<Connection[]> {
    return this.http.get<Connection[]>(`${this.baseUrl}/connections`);
  }
  getConnection(id: string): Observable<Connection> {
    return this.http.get<Connection>(`${this.baseUrl}/connections/${id}`);
  }
  createConnection(connection: Connection): Observable<Connection> {
    return this.http.post<Connection>(`${this.baseUrl}/connections`, connection);
  }
  updateConnection(id: string, connection: Connection): Observable<Connection> {
    return this.http.put<Connection>(`${this.baseUrl}/connections/${id}`, connection);
  }
  deleteConnection(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/connections/${id}`);
  }

  // Workloads
  getWorkloads(): Observable<Workload[]> {
    return this.http.get<Workload[]>(`${this.baseUrl}/workloads`);
  }
  getWorkload(id: string): Observable<Workload> {
    return this.http.get<Workload>(`${this.baseUrl}/workloads/${id}`);
  }
  createWorkload(workload: Workload): Observable<Workload> {
    return this.http.post<Workload>(`${this.baseUrl}/workloads`, workload);
  }
  updateWorkload(id: string, workload: Workload): Observable<Workload> {
    return this.http.put<Workload>(`${this.baseUrl}/workloads/${id}`, workload);
  }
  deleteWorkload(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/workloads/${id}`);
  }
}

