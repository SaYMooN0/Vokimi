namespace VokimiShared.src
{
    public record struct Err(string Message)
    {
        public Err(Exception ex) : this(ex.Message) { }
        public override string ToString() => Message;
        public static Err None => new(string.Empty);
        public bool NotNone() => this != None;
    }
}
