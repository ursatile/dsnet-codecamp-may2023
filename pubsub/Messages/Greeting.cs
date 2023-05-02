namespace Messages;
public class Greeting {
    public string Message { get;set;}
    public string MachineName { get;set;}
    public DateTimeOffset CreatedAt { get;set; }

    public Greeting(string message) {
        this.Message = message;
        this.MachineName = Environment.MachineName;
        this.CreatedAt = DateTimeOffset.UtcNow;
    }

    public override string ToString()
        => $"{Message} (from {MachineName} at {CreatedAt}";
}
