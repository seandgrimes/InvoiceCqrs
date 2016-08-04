namespace InvoiceCqrs.Visitors
{
    public interface IVisitable<in TVisitor>
    {
        void Accept(TVisitor visitor);
    }

    public interface IVisitable<in TVisitor, out TReturnValue>
    {
        TReturnValue Accept(TVisitor visitor);
    }
}
