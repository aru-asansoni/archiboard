using Neo4j.Driver;
using Archiboard.Api.Models;
using System.Text.Json;

namespace Archiboard.Api.Services;

public class Neo4jDataStore : IDataStore, IDisposable
{
    private readonly IDriver _driver;

    public Neo4jDataStore(string uri, string username, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
    }

    private async Task<IAsyncSession> GetSessionAsync()
    {
        return _driver.AsyncSession();
    }

    // Publishers
    public List<Publisher> GetPublishers() => GetPublishersAsync().Result;
    public async Task<List<Publisher>> GetPublishersAsync()
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync("MATCH (p:Publisher) RETURN p");
        var records = await result.ToListAsync();
        return records.Select(r => MapNodeToPublisher(r["p"].As<INode>())).ToList();
    }

    public Publisher? GetPublisher(Guid id) => GetPublisherAsync(id).Result;
    public async Task<Publisher?> GetPublisherAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync("MATCH (p:Publisher {id: $id}) RETURN p", new { id = id.ToString() });
        var record = await result.SingleOrDefaultAsync();
        return record == null ? null : MapNodeToPublisher(record["p"].As<INode>());
    }

    public Publisher CreatePublisher(Publisher publisher)
    {
        if (publisher.Id == Guid.Empty) publisher.Id = Guid.NewGuid();
        CreatePublisherAsync(publisher).Wait();
        return publisher;
    }

    public async Task<Publisher> CreatePublisherAsync(Publisher publisher)
    {
        using var session = await GetSessionAsync();
        await session.RunAsync(
            "CREATE (p:Publisher {id: $id, name: $name}) RETURN p",
            new { id = publisher.Id.ToString(), name = publisher.Name });
        return publisher;
    }

    public Publisher? UpdatePublisher(Guid id, Publisher publisher)
    {
        publisher.Id = id;
        UpdatePublisherAsync(id, publisher).Wait();
        return publisher;
    }

    public async Task<Publisher?> UpdatePublisherAsync(Guid id, Publisher publisher)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (p:Publisher {id: $id}) SET p.name = $name RETURN p",
            new { id = id.ToString(), name = publisher.Name });
        var record = await result.SingleOrDefaultAsync();
        return record == null ? null : MapNodeToPublisher(record["p"].As<INode>());
    }

    public bool DeletePublisher(Guid id) => DeletePublisherAsync(id).Result;
    public async Task<bool> DeletePublisherAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (p:Publisher {id: $id}) DETACH DELETE p RETURN count(p) as deleted",
            new { id = id.ToString() });
        var record = await result.SingleAsync();
        return record["deleted"].As<long>() > 0;
    }

    private Publisher MapNodeToPublisher(INode node) => new()
    {
        Id = Guid.Parse(node["id"].As<string>()),
        Name = node["name"].As<string>()
    };

    // Similar implementations for other entities... (Brokers, Topics, etc.)
    // For brevity, I'll show the pattern and implement key ones

    // Brokers
    public List<Broker> GetBrokers() => GetBrokersAsync().Result;
    public async Task<List<Broker>> GetBrokersAsync()
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync("MATCH (b:Broker) RETURN b");
        var records = await result.ToListAsync();
        return records.Select(r => MapNodeToBroker(r["b"].As<INode>())).ToList();
    }

    public Broker? GetBroker(Guid id) => GetBrokerAsync(id).Result;
    public async Task<Broker?> GetBrokerAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync("MATCH (b:Broker {id: $id}) RETURN b", new { id = id.ToString() });
        var record = await result.SingleOrDefaultAsync();
        return record == null ? null : MapNodeToBroker(record["b"].As<INode>());
    }

    public Broker CreateBroker(Broker broker)
    {
        if (broker.Id == Guid.Empty) broker.Id = Guid.NewGuid();
        CreateBrokerAsync(broker).Wait();
        return broker;
    }

    public async Task<Broker> CreateBrokerAsync(Broker broker)
    {
        using var session = await GetSessionAsync();
        await session.RunAsync(
            "CREATE (b:Broker {id: $id, name: $name, version: $version, type: $type, clusterName: $clusterName}) RETURN b",
            new
            {
                id = broker.Id.ToString(),
                name = broker.Name,
                version = broker.Version,
                type = (int)broker.Type,
                clusterName = broker.ClusterName
            });
        return broker;
    }

    public Broker? UpdateBroker(Guid id, Broker broker)
    {
        broker.Id = id;
        UpdateBrokerAsync(id, broker).Wait();
        return broker;
    }

    public async Task<Broker?> UpdateBrokerAsync(Guid id, Broker broker)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (b:Broker {id: $id}) SET b.name = $name, b.version = $version, b.type = $type, b.clusterName = $clusterName RETURN b",
            new
            {
                id = id.ToString(),
                name = broker.Name,
                version = broker.Version,
                type = (int)broker.Type,
                clusterName = broker.ClusterName
            });
        var record = await result.SingleOrDefaultAsync();
        return record == null ? null : MapNodeToBroker(record["b"].As<INode>());
    }

    public bool DeleteBroker(Guid id) => DeleteBrokerAsync(id).Result;
    public async Task<bool> DeleteBrokerAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (b:Broker {id: $id}) DETACH DELETE b RETURN count(b) as deleted",
            new { id = id.ToString() });
        var record = await result.SingleAsync();
        return record["deleted"].As<long>() > 0;
    }

    private Broker MapNodeToBroker(INode node) => new()
    {
        Id = Guid.Parse(node["id"].As<string>()),
        Name = node["name"].As<string>(),
        Version = node["version"].As<string>(),
        Type = (BrokerType)node["type"].As<int>(),
        ClusterName = node["clusterName"].As<string>()
    };

    // Topics
    public List<Topic> GetTopics() => GetTopicsAsync().Result;
    public async Task<List<Topic>> GetTopicsAsync()
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (t:Topic)-[:SERVES]->(b:Broker) RETURN t, b.id as brokerId");
        var records = await result.ToListAsync();
        return records.Select(r => MapNodeToTopic(r["t"].As<INode>(), r["brokerId"].As<string>())).ToList();
    }

    public Topic? GetTopic(Guid id) => GetTopicAsync(id).Result;
    public async Task<Topic?> GetTopicAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (t:Topic {id: $id})-[:SERVES]->(b:Broker) RETURN t, b.id as brokerId",
            new { id = id.ToString() });
        var record = await result.SingleOrDefaultAsync();
        if (record == null) return null;
        var topic = MapNodeToTopic(record["t"].As<INode>(), record["brokerId"].As<string>());
        topic.Broker = await GetBrokerAsync(Guid.Parse(record["brokerId"].As<string>()));
        return topic;
    }

    public Topic CreateTopic(Topic topic)
    {
        if (topic.Id == Guid.Empty) topic.Id = Guid.NewGuid();
        CreateTopicAsync(topic).Wait();
        return topic;
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        using var session = await GetSessionAsync();
        await session.RunAsync(
            "MATCH (b:Broker {id: $brokerId}) CREATE (t:Topic {id: $id, name: $name, numberOfPartitions: $partitions})-[:SERVES]->(b) RETURN t",
            new
            {
                id = topic.Id.ToString(),
                name = topic.Name,
                partitions = topic.NumberOfPartitions,
                brokerId = topic.BrokerId.ToString()
            });
        return topic;
    }

    public Topic? UpdateTopic(Guid id, Topic topic)
    {
        topic.Id = id;
        UpdateTopicAsync(id, topic).Wait();
        return topic;
    }

    public async Task<Topic?> UpdateTopicAsync(Guid id, Topic topic)
    {
        using var session = await GetSessionAsync();
        await session.RunAsync(
            "MATCH (t:Topic {id: $id})-[r:SERVES]->() DELETE r",
            new { id = id.ToString() });
        var result = await session.RunAsync(
            "MATCH (t:Topic {id: $id}), (b:Broker {id: $brokerId}) SET t.name = $name, t.numberOfPartitions = $partitions CREATE (t)-[:SERVES]->(b) RETURN t",
            new
            {
                id = id.ToString(),
                name = topic.Name,
                partitions = topic.NumberOfPartitions,
                brokerId = topic.BrokerId.ToString()
            });
        var record = await result.SingleOrDefaultAsync();
        return record == null ? null : MapNodeToTopic(record["t"].As<INode>(), topic.BrokerId.ToString());
    }

    public bool DeleteTopic(Guid id) => DeleteTopicAsync(id).Result;
    public async Task<bool> DeleteTopicAsync(Guid id)
    {
        using var session = await GetSessionAsync();
        var result = await session.RunAsync(
            "MATCH (t:Topic {id: $id}) DETACH DELETE t RETURN count(t) as deleted",
            new { id = id.ToString() });
        var record = await result.SingleAsync();
        return record["deleted"].As<long>() > 0;
    }

    private Topic MapNodeToTopic(INode node, string brokerId) => new()
    {
        Id = Guid.Parse(node["id"].As<string>()),
        Name = node["name"].As<string>(),
        BrokerId = Guid.Parse(brokerId),
        NumberOfPartitions = node["numberOfPartitions"].As<int>()
    };

    // Placeholder implementations for other entities (simplified for brevity)
    // In production, implement all CRUD operations similar to above pattern

    public List<MessageSchema> GetMessageSchemas() => new();
    public MessageSchema? GetMessageSchema(Guid id) => null;
    public MessageSchema CreateMessageSchema(MessageSchema schema) { schema.Id = Guid.NewGuid(); return schema; }
    public MessageSchema? UpdateMessageSchema(Guid id, MessageSchema schema) => schema;
    public bool DeleteMessageSchema(Guid id) => false;

    public List<MessageConnection> GetMessageConnections() => new();
    public MessageConnection? GetMessageConnection(Guid id) => null;
    public MessageConnection CreateMessageConnection(MessageConnection connection) { connection.Id = Guid.NewGuid(); return connection; }
    public MessageConnection? UpdateMessageConnection(Guid id, MessageConnection connection) => connection;
    public bool DeleteMessageConnection(Guid id) => false;

    public List<Runtime> GetRuntimes() => new();
    public Runtime? GetRuntime(Guid id) => null;
    public Runtime CreateRuntime(Runtime runtime) { runtime.Id = Guid.NewGuid(); return runtime; }
    public Runtime? UpdateRuntime(Guid id, Runtime runtime) => runtime;
    public bool DeleteRuntime(Guid id) => false;

    public List<Database> GetDatabases() => new();
    public Database? GetDatabase(Guid id) => null;
    public Database CreateDatabase(Database database) { database.Id = Guid.NewGuid(); return database; }
    public Database? UpdateDatabase(Guid id, Database database) => database;
    public bool DeleteDatabase(Guid id) => false;

    public List<SoftwareComponent> GetSoftwareComponents() => new();
    public SoftwareComponent? GetSoftwareComponent(Guid id) => null;
    public SoftwareComponent CreateSoftwareComponent(SoftwareComponent component) { component.Id = Guid.NewGuid(); return component; }
    public SoftwareComponent? UpdateSoftwareComponent(Guid id, SoftwareComponent component) => component;
    public bool DeleteSoftwareComponent(Guid id) => false;

    public List<ApiDefinition> GetApis() => new();
    public ApiDefinition? GetApi(Guid id) => null;
    public ApiDefinition CreateApi(ApiDefinition api) { api.Id = Guid.NewGuid(); return api; }
    public ApiDefinition? UpdateApi(Guid id, ApiDefinition api) => api;
    public bool DeleteApi(Guid id) => false;

    public List<Connection> GetConnections() => new();
    public Connection? GetConnection(Guid id) => null;
    public Connection CreateConnection(Connection connection) { connection.Id = Guid.NewGuid(); return connection; }
    public Connection? UpdateConnection(Guid id, Connection connection) => connection;
    public bool DeleteConnection(Guid id) => false;

    public List<Workload> GetWorkloads() => new();
    public Workload? GetWorkload(Guid id) => null;
    public Workload CreateWorkload(Workload workload) { workload.Id = Guid.NewGuid(); return workload; }
    public Workload? UpdateWorkload(Guid id, Workload workload) => workload;
    public bool DeleteWorkload(Guid id) => false;

    public void Dispose()
    {
        _driver?.Dispose();
    }
}

