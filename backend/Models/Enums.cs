namespace Archiboard.Api.Models;

public enum Cardinality
{
    OneToOne,      // 1..1
    OneToMany,     // 1..*
    ManyToOne,     // *..1
    ZeroToOne,     // 0..1
    OneToZero      // 1..0
}

public enum ApiType
{
    REST,
    SOAP,
    GRPC,
    GraphQL,
    NotDefined
}

public enum SpecType
{
    OpenAPI,
    WSDL
}

public enum Exposure
{
    Public,
    Private
}

public enum BrokerType
{
    Kafka,
    RabbitMQ
}

public enum MessageFormat
{
    Avro,
    ProtoBuf,
    JSON
}

public enum MessageType
{
    Event,
    Command
}

public enum SoftwareComponentType
{
    Library,
    Framework
}

public enum Language
{
    CSharp,        // C#
    Java,
    Typescript,
    Javascript,
    Python,
    GoLang
}

public enum DatabaseTechnology
{
    MySQL,
    PostgreSQL,
    SQLServer,
    MongoDB,
    SQLite,
    Neo4J
}

