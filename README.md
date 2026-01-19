# Poke

A CLI tool to run connection checks against multiple server types. Quickly verify connectivity to SQL Servers and HTTP endpoints from the command line.

## Installation

Poke is distributed as a .NET tool. Install it globally with:

```bash
dotnet tool install --global Poke
```

**Requirements:** .NET 8.0 or later

## Quick Start

```bash
# Add a SQL Server to your configuration
poke new sqlserver --group "Production" --instance "Main DB" --data-source "localhost"

# Add an HTTP endpoint
poke new http --group "APIs" --instance "Health Check" --uri "https://api.example.com/health"

# Run connectivity checks with interactive selection
poke select

# Run all configured checks at once
poke all
```

## Commands

### `poke select`

Displays an interactive multi-select prompt to choose which servers to test. Results are shown in a live-updating table as checks complete.

```bash
poke select [OPTIONS]
```

**Options:**

| Option | Description |
|--------|-------------|
| `-c, --config <path>` | Path to a custom configuration file |
| `-b, --debug` | Show detailed exception information on failures |
| `-d, --dry-run` | Validate configuration without attempting connections |

### `poke all`

Runs connectivity checks against all configured servers without prompting for selection.

```bash
poke all [OPTIONS]
```

**Options:**

| Option | Description |
|--------|-------------|
| `-c, --config <path>` | Path to a custom configuration file |
| `-b, --debug` | Show detailed exception information on failures |
| `-d, --dry-run` | Validate configuration without attempting connections |

### `poke new sqlserver`

Add a new SQL Server to the configuration. Supports both connection string and individual parameter modes.

```bash
poke new sqlserver [OPTIONS]
```

**Options:**

| Option | Description |
|--------|-------------|
| `-g, --group <name>` | Group name for organizing servers |
| `-i, --instance <name>` | Display name for the server instance |
| `-d, --data-source <host>` | Data source (host name or IP address) |
| `-s, --connection-string <string>` | Full connection string (takes precedence over `--data-source`) |
| `-c, --config <path>` | Path to a custom configuration file |

If required options are omitted, you'll be prompted interactively.

**Examples:**

```bash
# Using data source (creates connection with Integrated Security)
poke new sqlserver -g "Production" -i "Orders DB" -d "sql-prod-01.example.com"

# Using a full connection string
poke new sqlserver -g "Development" -i "Local" -s "Server=localhost;Database=mydb;User Id=sa;Password=secret;"
```

### `poke new http`

Add a new HTTP endpoint to the configuration.

```bash
poke new http [OPTIONS]
```

**Options:**

| Option | Description |
|--------|-------------|
| `-g, --group <name>` | Group name for organizing servers |
| `-i, --instance <name>` | Display name for the endpoint |
| `-u, --uri <url>` | URI for the HTTP request |
| `-x, --insecure` | Skip SSL certificate validation |
| `-c, --config <path>` | Path to a custom configuration file |

**Examples:**

```bash
# Standard HTTPS endpoint
poke new http -g "APIs" -i "User Service" -u "https://api.example.com/health"

# Skip SSL validation for self-signed certificates
poke new http -g "Development" -i "Local API" -u "https://localhost:5001/health" --insecure
```

### `poke config`

Display the current configuration with all configured servers organized by type.

```bash
poke config [OPTIONS]
```

**Options:**

| Option | Description |
|--------|-------------|
| `-c, --config <path>` | Path to a custom configuration file |

## Configuration

Poke stores server configurations in a JSON file. By default, this is located in your user profile directory. Use the `--config` option on any command to specify an alternative configuration file.

### Configuration File Format

```json
{
  "version": 2,
  "servers": [
    {
      "$type": "SqlServer",
      "groupName": "Production",
      "instance": "Main Database",
      "connectionString": "Data Source=sql-prod-01;Integrated Security=True;Trust Server Certificate=True"
    },
    {
      "$type": "Http",
      "groupName": "APIs",
      "instance": "Health Check",
      "uri": "https://api.example.com/health",
      "insecure": false
    }
  ]
}
```

### Server Types

#### SQL Server

Tests connectivity by opening and closing a database connection.

| Property | Description |
|----------|-------------|
| `GroupName` | Organizational group for the server |
| `Instance` | Display name for the server |
| `ConnectionString` | ADO.NET connection string |

#### HTTP Server

Tests connectivity by sending an HTTP GET request and checking for a successful response.

| Property | Description |
|----------|-------------|
| `GroupName` | Organizational group for the endpoint |
| `Instance` | Display name for the endpoint |
| `Uri` | The URL to check |
| `Insecure` | Whether to skip SSL certificate validation |

## Building from Source

```bash
git clone https://github.com/jake-carpenter/poke.git
cd poke
dotnet build
dotnet run --project Poke -- select
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
