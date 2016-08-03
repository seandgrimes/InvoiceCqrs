using FluentMigrator;

namespace InvoiceCqrs.Migrations
{
    /// <summary>
    ///     Migration for AddExampleReport
    /// </summary>
    /// <remarks>
    ///     Date        Developer       Description
    ///     08/03/2016  SDG       
    /// </remarks>
    [Migration(201608031316)]
    public class Migration201608031316_AddExampleReport : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"INSERT INTO Reports.Report (Id, Name) VALUES('ed91e3cd-2f18-493d-9a8f-8ad47d780d52', 'Invoice Report')");
        }
    }
}
