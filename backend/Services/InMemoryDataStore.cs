using Archiboard.Api.Models;

namespace Archiboard.Api.Services;

public class InMemoryDataStore : IDataStore
{
    private readonly Dictionary<Guid, Publisher> _publishers = new();
    private readonly Dictionary<Guid, Broker> _brokers = new();
    private readonly Dictionary<Guid, Topic> _topics = new();
    private readonly Dictionary<Guid, MessageSchema> _messageSchemas = new();
    private readonly Dictionary<Guid, MessageConnection> _messageConnections = new();
    private readonly Dictionary<Guid, Runtime> _runtimes = new();
    private readonly Dictionary<Guid, Database> _databases = new();
    private readonly Dictionary<Guid, SoftwareComponent> _softwareComponents = new();
    private readonly Dictionary<Guid, ApiDefinition> _apis = new();
    private readonly Dictionary<Guid, Connection> _connections = new();
    private readonly Dictionary<Guid, Workload> _workloads = new();

    // Publishers
    public List<Publisher> GetPublishers() => _publishers.Values.ToList();
    public Publisher? GetPublisher(Guid id) => _publishers.GetValueOrDefault(id);
    public Publisher CreatePublisher(Publisher publisher)
    {
        if (publisher.Id == Guid.Empty) publisher.Id = Guid.NewGuid();
        _publishers[publisher.Id] = publisher;
        return publisher;
    }
    public Publisher? UpdatePublisher(Guid id, Publisher publisher)
    {
        if (!_publishers.ContainsKey(id)) return null;
        publisher.Id = id;
        _publishers[id] = publisher;
        return publisher;
    }
    public bool DeletePublisher(Guid id) => _publishers.Remove(id);

    // Brokers
    public List<Broker> GetBrokers() => _brokers.Values.ToList();
    public Broker? GetBroker(Guid id) => _brokers.GetValueOrDefault(id);
    public Broker CreateBroker(Broker broker)
    {
        if (broker.Id == Guid.Empty) broker.Id = Guid.NewGuid();
        _brokers[broker.Id] = broker;
        return broker;
    }
    public Broker? UpdateBroker(Guid id, Broker broker)
    {
        if (!_brokers.ContainsKey(id)) return null;
        broker.Id = id;
        _brokers[id] = broker;
        return broker;
    }
    public bool DeleteBroker(Guid id) => _brokers.Remove(id);

    // Topics
    public List<Topic> GetTopics() => _topics.Values.Select(t => ResolveTopic(t)).ToList();
    public Topic? GetTopic(Guid id) => _topics.ContainsKey(id) ? ResolveTopic(_topics[id]) : null;
    public Topic CreateTopic(Topic topic)
    {
        if (topic.Id == Guid.Empty) topic.Id = Guid.NewGuid();
        _topics[topic.Id] = topic;
        return topic;
    }
    public Topic? UpdateTopic(Guid id, Topic topic)
    {
        if (!_topics.ContainsKey(id)) return null;
        topic.Id = id;
        _topics[id] = topic;
        return topic;
    }
    public bool DeleteTopic(Guid id) => _topics.Remove(id);

    private Topic ResolveTopic(Topic topic)
    {
        if (topic.BrokerId != Guid.Empty)
        {
            topic.Broker = GetBroker(topic.BrokerId);
        }
        return topic;
    }

    // MessageSchemas
    public List<MessageSchema> GetMessageSchemas() => _messageSchemas.Values.ToList();
    public MessageSchema? GetMessageSchema(Guid id) => _messageSchemas.GetValueOrDefault(id);
    public MessageSchema CreateMessageSchema(MessageSchema schema)
    {
        if (schema.Id == Guid.Empty) schema.Id = Guid.NewGuid();
        _messageSchemas[schema.Id] = schema;
        return schema;
    }
    public MessageSchema? UpdateMessageSchema(Guid id, MessageSchema schema)
    {
        if (!_messageSchemas.ContainsKey(id)) return null;
        schema.Id = id;
        _messageSchemas[id] = schema;
        return schema;
    }
    public bool DeleteMessageSchema(Guid id) => _messageSchemas.Remove(id);

    // MessageConnections
    public List<MessageConnection> GetMessageConnections() => _messageConnections.Values.Select(mc => ResolveMessageConnection(mc)).ToList();
    public MessageConnection? GetMessageConnection(Guid id) => _messageConnections.ContainsKey(id) ? ResolveMessageConnection(_messageConnections[id]) : null;
    public MessageConnection CreateMessageConnection(MessageConnection connection)
    {
        if (connection.Id == Guid.Empty) connection.Id = Guid.NewGuid();
        _messageConnections[connection.Id] = connection;
        return connection;
    }
    public MessageConnection? UpdateMessageConnection(Guid id, MessageConnection connection)
    {
        if (!_messageConnections.ContainsKey(id)) return null;
        connection.Id = id;
        _messageConnections[id] = connection;
        return connection;
    }
    public bool DeleteMessageConnection(Guid id) => _messageConnections.Remove(id);

    private MessageConnection ResolveMessageConnection(MessageConnection connection)
    {
        if (connection.TopicId != Guid.Empty)
        {
            connection.Topic = GetTopic(connection.TopicId);
        }
        connection.Messages = connection.MessageSchemaIds
            .Select(id => GetMessageSchema(id))
            .Where(m => m != null)
            .Cast<MessageSchema>()
            .ToList();
        return connection;
    }

    // Runtimes
    public List<Runtime> GetRuntimes() => _runtimes.Values.ToList();
    public Runtime? GetRuntime(Guid id) => _runtimes.GetValueOrDefault(id);
    public Runtime CreateRuntime(Runtime runtime)
    {
        if (runtime.Id == Guid.Empty) runtime.Id = Guid.NewGuid();
        _runtimes[runtime.Id] = runtime;
        return runtime;
    }
    public Runtime? UpdateRuntime(Guid id, Runtime runtime)
    {
        if (!_runtimes.ContainsKey(id)) return null;
        runtime.Id = id;
        _runtimes[id] = runtime;
        return runtime;
    }
    public bool DeleteRuntime(Guid id) => _runtimes.Remove(id);

    // Databases
    public List<Database> GetDatabases() => _databases.Values.ToList();
    public Database? GetDatabase(Guid id) => _databases.GetValueOrDefault(id);
    public Database CreateDatabase(Database database)
    {
        if (database.Id == Guid.Empty) database.Id = Guid.NewGuid();
        _databases[database.Id] = database;
        return database;
    }
    public Database? UpdateDatabase(Guid id, Database database)
    {
        if (!_databases.ContainsKey(id)) return null;
        database.Id = id;
        _databases[id] = database;
        return database;
    }
    public bool DeleteDatabase(Guid id) => _databases.Remove(id);

    // SoftwareComponents
    public List<SoftwareComponent> GetSoftwareComponents() => _softwareComponents.Values.Select(sc => ResolveSoftwareComponent(sc)).ToList();
    public SoftwareComponent? GetSoftwareComponent(Guid id) => _softwareComponents.ContainsKey(id) ? ResolveSoftwareComponent(_softwareComponents[id]) : null;
    public SoftwareComponent CreateSoftwareComponent(SoftwareComponent component)
    {
        if (component.Id == Guid.Empty) component.Id = Guid.NewGuid();
        _softwareComponents[component.Id] = component;
        return component;
    }
    public SoftwareComponent? UpdateSoftwareComponent(Guid id, SoftwareComponent component)
    {
        if (!_softwareComponents.ContainsKey(id)) return null;
        component.Id = id;
        _softwareComponents[id] = component;
        return component;
    }
    public bool DeleteSoftwareComponent(Guid id) => _softwareComponents.Remove(id);

    private SoftwareComponent ResolveSoftwareComponent(SoftwareComponent component)
    {
        if (component.PublisherId.HasValue)
        {
            component.PublishedBy = GetPublisher(component.PublisherId.Value);
        }
        component.Components = component.ComponentIds
            .Select(id => GetSoftwareComponent(id))
            .Where(c => c != null)
            .Cast<SoftwareComponent>()
            .ToList();
        return component;
    }

    // APIs
    public List<ApiDefinition> GetApis() => _apis.Values.ToList();
    public ApiDefinition? GetApi(Guid id) => _apis.GetValueOrDefault(id);
    public ApiDefinition CreateApi(ApiDefinition api)
    {
        if (api.Id == Guid.Empty) api.Id = Guid.NewGuid();
        _apis[api.Id] = api;
        return api;
    }
    public ApiDefinition? UpdateApi(Guid id, ApiDefinition api)
    {
        if (!_apis.ContainsKey(id)) return null;
        api.Id = id;
        _apis[id] = api;
        return api;
    }
    public bool DeleteApi(Guid id) => _apis.Remove(id);

    // Connections
    public List<Connection> GetConnections() => _connections.Values.ToList();
    public Connection? GetConnection(Guid id) => _connections.GetValueOrDefault(id);
    public Connection CreateConnection(Connection connection)
    {
        if (connection.Id == Guid.Empty) connection.Id = Guid.NewGuid();
        _connections[connection.Id] = connection;
        return connection;
    }
    public Connection? UpdateConnection(Guid id, Connection connection)
    {
        if (!_connections.ContainsKey(id)) return null;
        connection.Id = id;
        _connections[id] = connection;
        return connection;
    }
    public bool DeleteConnection(Guid id) => _connections.Remove(id);

    // Workloads
    public List<Workload> GetWorkloads() => _workloads.Values.Select(w => ResolveWorkload(w)).ToList();
    public Workload? GetWorkload(Guid id) => _workloads.ContainsKey(id) ? ResolveWorkload(_workloads[id]) : null;
    public Workload CreateWorkload(Workload workload)
    {
        if (workload.Id == Guid.Empty) workload.Id = Guid.NewGuid();
        _workloads[workload.Id] = workload;
        return workload;
    }
    public Workload? UpdateWorkload(Guid id, Workload workload)
    {
        if (!_workloads.ContainsKey(id)) return null;
        workload.Id = id;
        _workloads[id] = workload;
        return workload;
    }
    public bool DeleteWorkload(Guid id) => _workloads.Remove(id);

    private Workload ResolveWorkload(Workload workload)
    {
        if (workload.RuntimeId.HasValue)
        {
            workload.Runtime = GetRuntime(workload.RuntimeId.Value);
        }
        workload.SoftwareComponents = workload.SoftwareComponentIds
            .Select(id => GetSoftwareComponent(id))
            .Where(sc => sc != null)
            .Cast<SoftwareComponent>()
            .ToList();
        workload.APIsExposed = workload.APIsExposedIds
            .Select(id => GetApi(id))
            .Where(api => api != null)
            .Cast<ApiDefinition>()
            .ToList();
        workload.APIsInvoked = workload.APIsInvokedIds
            .Select(id => GetApi(id))
            .Where(api => api != null)
            .Cast<ApiDefinition>()
            .ToList();
        workload.ConsumeMessageFrom = workload.ConsumeMessageFromIds
            .Select(id => GetMessageConnection(id))
            .Where(mc => mc != null)
            .Cast<MessageConnection>()
            .ToList();
        workload.ProduceMessageTo = workload.ProduceMessageToIds
            .Select(id => GetMessageConnection(id))
            .Where(mc => mc != null)
            .Cast<MessageConnection>()
            .ToList();
        workload.Databases = workload.DatabaseIds
            .Select(id => GetDatabase(id))
            .Where(db => db != null)
            .Cast<Database>()
            .ToList();
        return workload;
    }
}

