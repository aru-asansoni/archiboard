#!/usr/bin/env python3
"""
Archiboard Neo4j Seeder
Creates entities based on the README diagram structure
"""

from neo4j import GraphDatabase
import uuid
import os
import sys

# Neo4j connection details
NEO4J_URI = os.getenv("NEO4J_URI", "bolt://localhost:7687")
NEO4J_USERNAME = os.getenv("NEO4J_USERNAME", "neo4j")
NEO4J_PASSWORD = os.getenv("NEO4J_PASSWORD", "archiboard123")

# Entity IDs (using consistent UUIDs for relationships)
BROKER_KAFKA_ID = str(uuid.uuid4())
TOPIC_TOPIC_A_ID = str(uuid.uuid4())
WORKLOAD_APP001_ID = str(uuid.uuid4())
RUNTIME_K8S001_ID = str(uuid.uuid4())
RUNTIME_VM001A_ID = str(uuid.uuid4())
RUNTIME_VM001B_ID = str(uuid.uuid4())
RUNTIME_VM001C_ID = str(uuid.uuid4())
RUNTIME_CLUSTER001_ID = str(uuid.uuid4())
API_API001_ID = str(uuid.uuid4())
DATABASE_DB001_ID = str(uuid.uuid4())
MESSAGE_CONNECTION_CONSUME_ID = str(uuid.uuid4())
MESSAGE_CONNECTION_PRODUCE_ID = str(uuid.uuid4())


def clear_database(driver):
    """Clear all nodes and relationships"""
    with driver.session() as session:
        session.run("MATCH (n) DETACH DELETE n")
        print("✓ Cleared database")


def create_broker(driver, broker_id, name, version, broker_type, cluster_name):
    """Create a Broker node"""
    with driver.session() as session:
        session.run("""
            CREATE (b:Broker {
                id: $id,
                name: $name,
                version: $version,
                type: $type,
                clusterName: $clusterName
            })
        """, id=broker_id, name=name, version=version, type=broker_type, clusterName=cluster_name)
        print(f"✓ Created Broker: {name}")


def create_topic(driver, topic_id, name, broker_id, partitions):
    """Create a Topic node and link to Broker"""
    with driver.session() as session:
        session.run("""
            MATCH (b:Broker {id: $brokerId})
            CREATE (t:Topic {
                id: $id,
                name: $name,
                numberOfPartitions: $partitions
            })
            CREATE (t)-[:SERVES]->(b)
        """, id=topic_id, name=name, brokerId=broker_id, partitions=partitions)
        print(f"✓ Created Topic: {name}")


def create_runtime(driver, runtime_id, name, version, vendor, lts, eol=None):
    """Create a Runtime node"""
    with driver.session() as session:
        session.run("""
            CREATE (r:Runtime {
                id: $id,
                name: $name,
                version: $version,
                vendor: $vendor,
                lts: $lts,
                eol: $eol
            })
        """, id=runtime_id, name=name, version=version, vendor=vendor, lts=lts, eol=eol)
        print(f"✓ Created Runtime: {name}")


def create_workload(driver, workload_id, name, repo_url, tag, runtime_id=None):
    """Create a Workload node"""
    with driver.session() as session:
        if runtime_id:
            session.run("""
                MATCH (r:Runtime {id: $runtimeId})
                CREATE (w:Workload {
                    id: $id,
                    name: $name,
                    repoUrl: $repoUrl,
                    tag: $tag
                })
                CREATE (w)-[:RUNS_ON]->(r)
            """, id=workload_id, name=name, repoUrl=repo_url, tag=tag, runtimeId=runtime_id)
        else:
            session.run("""
                CREATE (w:Workload {
                    id: $id,
                    name: $name,
                    repoUrl: $repoUrl,
                    tag: $tag
                })
            """, id=workload_id, name=name, repoUrl=repo_url, tag=tag)
        print(f"✓ Created Workload: {name}")


def create_api(driver, api_id, name, version, api_type, service_url, spec_url, spec_type, exposure):
    """Create an API node"""
    with driver.session() as session:
        session.run("""
            CREATE (a:Api {
                id: $id,
                name: $name,
                version: $version,
                type: $type,
                serviceUrl: $serviceUrl,
                specUrl: $specUrl,
                specType: $specType,
                exposure: $exposure
            })
        """, id=api_id, name=name, version=version, type=api_type,
            serviceUrl=service_url, specUrl=spec_url, specType=spec_type, exposure=exposure)
        print(f"✓ Created API: {name}")


def create_database(driver, db_id, name, version, technology):
    """Create a Database node"""
    with driver.session() as session:
        session.run("""
            CREATE (d:Database {
                id: $id,
                name: $name,
                version: $version,
                technology: $technology
            })
        """, id=db_id, name=name, version=version, technology=technology)
        print(f"✓ Created Database: {name}")


def create_message_connection(driver, mc_id, topic_id):
    """Create a MessageConnection node"""
    with driver.session() as session:
        session.run("""
            MATCH (t:Topic {id: $topicId})
            CREATE (mc:MessageConnection {
                id: $id
            })
            CREATE (mc)-[:CONNECTS_TO]->(t)
        """, id=mc_id, topicId=topic_id)
        print(f"✓ Created MessageConnection: {mc_id}")


def create_relationships(driver):
    """Create relationships between entities"""
    with driver.session() as session:
        # Topic is-consumed-by app001 (workload consumes from topic)
        session.run("""
            MATCH (t:Topic {id: $topicId}), (w:Workload {id: $workloadId})
            CREATE (t)-[:CONSUMED_BY]->(w)
            CREATE (w)-[:CONSUMES]->(t)
        """, topicId=TOPIC_TOPIC_A_ID, workloadId=WORKLOAD_APP001_ID)
        print("✓ Created relationship: Topic consumed-by Workload")

        # app001 is-hosted-on k8s001
        session.run("""
            MATCH (w:Workload {id: $workloadId}), (r:Runtime {id: $runtimeId})
            CREATE (w)-[:HOSTED_ON]->(r)
            CREATE (r)-[:HOSTS]->(w)
        """, workloadId=WORKLOAD_APP001_ID, runtimeId=RUNTIME_K8S001_ID)
        print("✓ Created relationship: Workload hosted-on Runtime")

        # Kafka is-hosted-on vm001a
        session.run("""
            MATCH (b:Broker {id: $brokerId}), (r:Runtime {id: $runtimeId})
            CREATE (b)-[:HOSTED_ON]->(r)
            CREATE (r)-[:HOSTS]->(b)
        """, brokerId=BROKER_KAFKA_ID, runtimeId=RUNTIME_VM001A_ID)
        print("✓ Created relationship: Broker hosted-on Runtime")

        # k8s001 is-hosted-on cluster001
        session.run("""
            MATCH (r1:Runtime {id: $runtime1Id}), (r2:Runtime {id: $runtime2Id})
            CREATE (r1)-[:HOSTED_ON]->(r2)
            CREATE (r2)-[:HOSTS]->(r1)
        """, runtime1Id=RUNTIME_K8S001_ID, runtime2Id=RUNTIME_CLUSTER001_ID)
        print("✓ Created relationship: Runtime hosted-on Cluster")

        # cluster001 comprises vm001b and vm001c
        session.run("""
            MATCH (c:Runtime {id: $clusterId}), (vm:Runtime {id: $vmId})
            CREATE (c)-[:COMPRISES]->(vm)
            CREATE (vm)-[:COMPRISED_OF]->(c)
        """, clusterId=RUNTIME_CLUSTER001_ID, vmId=RUNTIME_VM001B_ID)
        session.run("""
            MATCH (c:Runtime {id: $clusterId}), (vm:Runtime {id: $vmId})
            CREATE (c)-[:COMPRISES]->(vm)
            CREATE (vm)-[:COMPRISED_OF]->(c)
        """, clusterId=RUNTIME_CLUSTER001_ID, vmId=RUNTIME_VM001C_ID)
        print("✓ Created relationships: Cluster comprises VMs")

        # Workload exposes API
        session.run("""
            MATCH (w:Workload {id: $workloadId}), (a:Api {id: $apiId})
            CREATE (w)-[:EXPOSES]->(a)
            CREATE (a)-[:EXPOSED_BY]->(w)
        """, workloadId=WORKLOAD_APP001_ID, apiId=API_API001_ID)
        print("✓ Created relationship: Workload exposes API")

        # Workload uses Database
        session.run("""
            MATCH (w:Workload {id: $workloadId}), (d:Database {id: $dbId})
            CREATE (w)-[:USES_DB]->(d)
            CREATE (d)-[:USED_BY]->(w)
        """, workloadId=WORKLOAD_APP001_ID, dbId=DATABASE_DB001_ID)
        print("✓ Created relationship: Workload uses Database")


def seed_database():
    """Main seeding function"""
    print("Connecting to Neo4j...")
    driver = GraphDatabase.driver(NEO4J_URI, auth=(NEO4J_USERNAME, NEO4J_PASSWORD))
    
    try:
        # Verify connection
        driver.verify_connectivity()
        print("✓ Connected to Neo4j\n")
        
        # Clear existing data
        print("Clearing existing data...")
        clear_database(driver)
        print()
        
        # Create entities based on README diagram
        print("Creating entities...\n")
        
        # 1. Create Broker (Kafka)
        create_broker(driver, BROKER_KAFKA_ID, "kafka", "1234", 0, "kafka-cluster")
        
        # 2. Create Topic (topic-a)
        create_topic(driver, TOPIC_TOPIC_A_ID, "topic-a", BROKER_KAFKA_ID, 3)
        
        # 3. Create Runtimes
        create_runtime(driver, RUNTIME_K8S001_ID, "k8s-001", "1.28", "Kubernetes", True, None)
        create_runtime(driver, RUNTIME_VM001A_ID, "vm-001a", "2022", "VMware", True, None)
        create_runtime(driver, RUNTIME_VM001B_ID, "vm-001b", "2022", "VMware", True, None)
        create_runtime(driver, RUNTIME_VM001C_ID, "vm-001c", "2022", "VMware", True, None)
        create_runtime(driver, RUNTIME_CLUSTER001_ID, "cluster-001", "1.0", "Custom", True, None)
        
        # 4. Create Workload (app-001)
        create_workload(driver, WORKLOAD_APP001_ID, "app-001", "https://github.com/example/app-001", "v1.0.0", RUNTIME_K8S001_ID)
        
        # 5. Create API
        create_api(driver, API_API001_ID, "api-001", "1.0.0", 0, "http://api-001.example.com", "http://api-001.example.com/openapi.json", 0, 0)
        
        # 6. Create Database
        create_database(driver, DATABASE_DB001_ID, "db-001", "15.0", 5)  # Neo4J
        
        # 7. Create Message Connections
        create_message_connection(driver, MESSAGE_CONNECTION_CONSUME_ID, TOPIC_TOPIC_A_ID)
        create_message_connection(driver, MESSAGE_CONNECTION_PRODUCE_ID, TOPIC_TOPIC_A_ID)
        
        # 8. Create relationships
        print("\nCreating relationships...\n")
        create_relationships(driver)
        
        print("\n✓ Database seeding completed successfully!")
        print(f"\nCreated entities:")
        print(f"  - 1 Broker (Kafka)")
        print(f"  - 1 Topic (topic-a)")
        print(f"  - 5 Runtimes (k8s-001, vm-001a, vm-001b, vm-001c, cluster-001)")
        print(f"  - 1 Workload (app-001)")
        print(f"  - 1 API (api-001)")
        print(f"  - 1 Database (db-001)")
        print(f"  - 2 Message Connections")
        
    except Exception as e:
        print(f"Error: {e}")
        sys.exit(1)
    finally:
        driver.close()


if __name__ == "__main__":
    seed_database()

