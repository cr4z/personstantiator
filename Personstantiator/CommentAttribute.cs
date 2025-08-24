namespace Personstantiator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class CommentAttribute : Attribute
    {
        public string Text { get; }
        public CommentAttribute(string text) => Text = text;
    }
}
