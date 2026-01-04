# Archiboard Docker Compose

This Docker Compose file sets up the complete Archiboard application stack.

## Services

1. **Neo4j** (port 7474, 7687)
   - Graph database for storing architecture entities
   - Web UI: http://localhost:7474
   - Username: `neo4j`
   - Password: `archiboard123`

2. **Backend** (port 5000, 5001)
   - ASP.NET Core 10 Web API
   - API: http://localhost:5000

3. **Frontend** (port 4200)
   - Angular 21 application
   - UI: http://localhost:4200

4. **Seeder** (profile: seeder)
   - Python script to seed Neo4j with initial data
   - Run with: `docker-compose --profile seeder run seeder`

## Usage

### Start all services

```bash
docker-compose up -d
```

### Run seeder

```bash
docker-compose --profile seeder run seeder
```

### Stop all services

```bash
docker-compose down
```

### View logs

```bash
docker-compose logs -f [service-name]
```

### Access Neo4j Browser

Open http://localhost:7474 in your browser
- Username: `neo4j`
- Password: `archiboard123`

## Development

For development, you can mount volumes to enable hot-reload:
- Backend: Mount `./backend` to `/app`
- Frontend: Mount `./frontend` to `/app`

## Environment Variables

Backend and seeder use these environment variables:
- `Neo4j__Uri`: Neo4j connection URI
- `Neo4j__Username`: Neo4j username
- `Neo4j__Password`: Neo4j password

These are configured in `docker-compose.yml` and can be overridden via `.env` file.

