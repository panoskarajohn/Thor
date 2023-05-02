namespace Shared.Logging.Options;

public class ConsoleOptions
{
    public bool Enabled { get; set; }
}

public class FileOptions
{
    public bool Enabled { get; set; }
    public string Path { get; set; }
    public string Interval { get; set; }
}

public class LoggerOptions
{
    public string Level { get; set; }
    public ConsoleOptions Console { get; set; }
    public FileOptions File { get; set; }
    public SeqOptions Seq { get; set; }
    public IDictionary<string, string> Overrides { get; set; }
    public IEnumerable<string> ExcludePaths { get; set; }
    public IEnumerable<string> ExcludeProperties { get; set; }
    public IDictionary<string, object> Tags { get; set; }
}

public class SeqOptions
{
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string ApiKey { get; set; }
}