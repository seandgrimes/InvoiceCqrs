namespace InvoiceCqrs.Persistence.EventStore
{
    public interface ISubscribe<in TEvent>
    {
        void Receive(TEvent evt);
    }
}
