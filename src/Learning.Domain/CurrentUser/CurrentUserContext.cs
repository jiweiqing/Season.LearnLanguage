namespace Learning.Domain
{
    public class CurrentUserContext
    {
        public long? Id { get; private set; }

        public string? UserName { get; private set; }

        public void SetValue(long? id)
        {
            SetValue(id, null);
        }

        public void SetValue(long? id, string? userName)
        {
            SetValue(id, userName);
        }
    }
}
