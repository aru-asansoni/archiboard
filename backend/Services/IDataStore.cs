using Archiboard.Api.Models;

namespace Archiboard.Api.Services;

public interface IDataStore
{
    // Publishers
    List<Publisher> GetPublishers();
    Publisher? GetPublisher(Guid id);
    Publisher CreatePublisher(Publisher publisher);
    Publisher? UpdatePublisher(Guid id, Publisher publisher);
    bool DeletePublisher(Guid id);

    // Brokers
    List<Broker> GetBrokers();
    Broker? GetBroker(Guid id);
    Broker CreateBroker(Broker broker);
    Broker? UpdateBroker(Guid id, Broker broker);
    bool DeleteBroker(Guid id);

    // Topics
    List<Topic> GetTopics();
    Topic? GetTopic(Guid id);
    Topic CreateTopic(Topic topic);
    Topic? UpdateTopic(Guid id, Topic topic);
    bool DeleteTopic(Guid id);

    // MessageSchemas
    List<MessageSchema> GetMessageSchemas();
    MessageSchema? GetMessageSchema(Guid id);
    MessageSchema CreateMessageSchema(MessageSchema schema);
    MessageSchema? UpdateMessageSchema(Guid id, MessageSchema schema);
    bool DeleteMessageSchema(Guid id);

    // MessageConnections
    List<MessageConnection> GetMessageConnections();
    MessageConnection? GetMessageConnection(Guid id);
    MessageConnection CreateMessageConnection(MessageConnection connection);
    MessageConnection? UpdateMessageConnection(Guid id, MessageConnection connection);
    bool DeleteMessageConnection(Guid id);

    // Runtimes
    List<Runtime> GetRuntimes();
    Runtime? GetRuntime(Guid id);
    Runtime CreateRuntime(Runtime runtime);
    Runtime? UpdateRuntime(Guid id, Runtime runtime);
    bool DeleteRuntime(Guid id);

    // Databases
    List<Database> GetDatabases();
    Database? GetDatabase(Guid id);
    Database CreateDatabase(Database database);
    Database? UpdateDatabase(Guid id, Database database);
    bool DeleteDatabase(Guid id);

    // SoftwareComponents
    List<SoftwareComponent> GetSoftwareComponents();
    SoftwareComponent? GetSoftwareComponent(Guid id);
    SoftwareComponent CreateSoftwareComponent(SoftwareComponent component);
    SoftwareComponent? UpdateSoftwareComponent(Guid id, SoftwareComponent component);
    bool DeleteSoftwareComponent(Guid id);

    // APIs
    List<ApiDefinition> GetApis();
    ApiDefinition? GetApi(Guid id);
    ApiDefinition CreateApi(ApiDefinition api);
    ApiDefinition? UpdateApi(Guid id, ApiDefinition api);
    bool DeleteApi(Guid id);

    // Connections
    List<Connection> GetConnections();
    Connection? GetConnection(Guid id);
    Connection CreateConnection(Connection connection);
    Connection? UpdateConnection(Guid id, Connection connection);
    bool DeleteConnection(Guid id);

    // Workloads
    List<Workload> GetWorkloads();
    Workload? GetWorkload(Guid id);
    Workload CreateWorkload(Workload workload);
    Workload? UpdateWorkload(Guid id, Workload workload);
    bool DeleteWorkload(Guid id);
}

