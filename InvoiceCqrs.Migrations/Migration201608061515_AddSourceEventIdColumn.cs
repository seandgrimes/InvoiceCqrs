using FluentMigrator;

namespace InvoiceCqrs.Migrations
{
    /// <summary>
    ///     Migration for AddParentEventGuidColumn
    /// </summary>
    /// <remarks>
    ///     Date        Developer       Description
    ///     08/06/2016  SDG       
    /// </remarks>
    [Migration(201608061515)]
    public class Migration201608061515_AddSourceEventIdColumn : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("Event")
                .InSchema("EventStore")
                .AddColumn("SourceEventId")
                    .AsGuid()
                    .WithColumnDescription("Id of the event this event was generated in response to")
                    .Nullable()
                    .Indexed()
                    .ForeignKey("FK_EventStore.Event.SourceEventId_EventStore.Event.Id", "EventStore", "Event", "Id");
        }
    }
}
