namespace TN_AI_NOTES_DEMO
{
    public interface IArtificialIntelligence
    {
        public Task<string> Query(string prompt);
    }
}
