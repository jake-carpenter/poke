using System.Text.Json.Serialization;
using Poke.Models;

namespace Poke.Runners;

public record SqlServer : Server
{
  [JsonIgnore]
  public override string Type => "SqlServer";
}