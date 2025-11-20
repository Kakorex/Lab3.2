namespace DAL
{
    public interface IBehavior
    {
        string Do();
    }

    public class DecitePoems : IBehavior
    {
        public string Do() { return "deciting poems"; }
    }
    public class Study : IBehavior
    {
        public string Do() { return "studying"; }
    }
    public class PlayFootball : IBehavior
    {
        public string Do() { return "playing football"; }
    }
    public class PracticeLaw : IBehavior
    {
        public string Do() { return "practicing law"; }
    }
}