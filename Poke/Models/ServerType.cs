namespace Poke.Models;

/// <summary>
/// Defines the types of servers that can be monitored.
/// </summary>
public enum ServerType
{
    /// <summary>
    /// A Microsoft SQL Server database server.
    /// </summary>
    SqlServer,

    /// <summary>
    /// An HTTP web server.
    /// </summary>
    Http
}