# Archiboard Neo4j Seeder

This seeder creates initial architecture entities in Neo4j based on the diagram described in the root README file.

## Entities Created

Based on the README diagram, the seeder creates:

1. **Broker**: Kafka (version 1234)
2. **Topic**: topic-a (connected to Kafka broker)
3. **Runtimes**: 
   - k8s-001 (Kubernetes cluster)
   - vm-001a, vm-001b, vm-001c (Virtual machines)
   - cluster-001 (Cluster)
4. **Workload**: app-001 (Application)
5. **API**: api-001 (REST API)
6. **Database**: db-001 (Neo4j database)

## Relationships Created

- Topic serves → Broker (Kafka)
- Topic consumed-by → Workload (app-001)
- Workload hosted-on → Runtime (k8s-001)
- Broker hosted-on → Runtime (vm-001a)
- Runtime hosted-on → Cluster (k8s-001 on cluster-001)
- Cluster comprises → VMs (vm-001b, vm-001c)
- Workload exposes → API
- Workload uses → Database

## Usage

### Using Docker Compose

Run the seeder with Docker Compose:

```bash
docker-compose --profile seeder run seeder
```

### Running Locally

1. Ensure Neo4j is running and accessible
2. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```
3. Set environment variables:
   ```bash
   export NEO4J_URI=bolt://localhost:7687
   export NEO4J_USERNAME=neo4j
   export NEO4J_PASSWORD=archiboard123
   ```
4. Run the seeder:
   ```bash
   python seed.py
   ```

## Environment Variables

- `NEO4J_URI`: Neo4j connection URI (default: `bolt://localhost:7687`)
- `NEO4J_USERNAME`: Neo4j username (default: `neo4j`)
- `NEO4J_PASSWORD`: Neo4j password (default: `archiboard123`)

## Verification

After seeding, you can verify the data in Neo4j Browser:

1. Open Neo4j Browser at `http://localhost:7474`
2. Run query: `MATCH (n) RETURN n LIMIT 100`
3. Run query: `MATCH (n)-[r]->(m) RETURN n, r, m LIMIT 50`

This will show all nodes and relationships created by the seeder.

