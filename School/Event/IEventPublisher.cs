namespace School.Event
{
    public interface IEventPublisher
    {
        void Publish(IEvent domainEvent);
    }
}
