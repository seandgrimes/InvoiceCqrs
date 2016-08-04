using System.Collections.Generic;
using FluentMigrator;

namespace InvoiceCqrs.Migrations
{
    /// <summary>
    ///     Migration for CreateEventStreams
    /// </summary>
    /// <remarks>
    ///     Date        Developer       Description
    ///     08/04/2016  SDG       
    /// </remarks>
    [Migration(201608040827)]
    public class Migration201608040827_CreateEventStreams : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO EventStore.Stream (Id, Name) 
                VALUES
                    ('902e2636-5a4f-11e6-a7ab-b8ca3a940de8', 'Companies'),
                    ('902e2637-5a4f-11e6-a7ab-b8ca3a940de8', 'Invoices'),
                    ('902e2638-5a4f-11e6-a7ab-b8ca3a940de8', 'Reports'),
                    ('902e2639-5a4f-11e6-a7ab-b8ca3a940de8', 'Users');"  );
        }
    }
}
