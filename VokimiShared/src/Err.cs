namespace VokimiShared.src
{
    public record struct Err(string Message)
    {
        public override string ToString() => Message;
        public static Err None => new(string.Empty);
        public bool NotNone() => this != None;
    }
}
