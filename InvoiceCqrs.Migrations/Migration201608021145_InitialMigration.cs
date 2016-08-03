using System;
using FluentMigrator;

namespace InvoiceCqrs.Migrations
{
    /// <summary>
    ///     Migration for InitialMigration
    /// </summary>
    /// <remarks>
    ///     Date        Developer       Description
    ///     08/02/2016  SDG       
    /// </remarks>
    [Migration(201608021145)]
    public class Migration201608021145_InitialMigration : ForwardOnlyMigration
    {
        public override void Up()
        {
            CreateSchemas();
            CreateTables();
        }

        private void CreateTables()
        {
            CreateTableTable();
            CreateCompanyTable();
            CreateUserTable();
            CreateReportTable();

            CreateStreamTable();
            CreateEventTable();
            CreateEvetMetadataTable();

            CreateInvoiceTable();
            CreateLineItemTable();
            CreateGeneralLedgerTable();
            CreatePaymentTable();
        }

        private void CreateCompanyTable()
        {
            Create.Table("Company")
                .WithDescription("Table for holding companies")
                .InSchema("Companies")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Name")
                    .AsString(255)
                    .NotNullable()
                    .Unique()
                .WithColumn("Addr1")
                .AsString(255)
                    .NotNullable()
                .WithColumn("Addr2")
                    .AsString(255)
                .WithColumn("City")
                    .AsString(255)
                    .NotNullable()
                .WithColumn("State")
                    .AsString(5)
                    .NotNullable()
                    .Indexed()
                .WithColumn("ZipCode")
                    .AsString(15)
                    .NotNullable();
        }

        private void CreateEventTable()
        {
            Create.Table("Event")
                .WithDescription("Contains information about the events that have occurred in a stream")
                .InSchema("EventStore")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("CorrelationId")
                    .AsGuid()
                    .NotNullable()
                    .Indexed()
                .WithColumn("EventDate")
                    .AsDateTime()
                    .NotNullable()
                    .WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("EventType")
                    .AsString(1000)
                    .NotNullable()
                .WithColumn("IsDispatched")
                    .AsBoolean()
                    .NotNullable()
                    .WithDefaultValue(false)
                .WithColumn("Json")
                    .AsString(int.MaxValue)
                    .NotNullable()
                .WithColumn("StreamId")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("EventStore.Event.StreamId", "EventStore.Stream.Id"), "EventStore", "Stream", "Id");

            Create.Index("Idx_EventStore.Event_EventType_IsDispatched_StreamId")
                .OnTable("Event").InSchema("EventStore")
                .OnColumn("EventType").Ascending()
                .OnColumn("IsDispatched").Ascending()
                .OnColumn("StreamId");
        }

        private void CreateEvetMetadataTable()
        {
            Create.Table("EventMetadata")
                .WithDescription("Contains metadata about an event that has occurred, useful for efficiently searching events")
                .InSchema("EventStore")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("EventId")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("EventStore.EventMetadata.EventId", "EventStore.Event.Id"), "EventStore", "Event", "Id")
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Value").AsString(int.MaxValue).NotNullable();

            Create.Index("Idx_EventStore.EventMetadata_EventId_Key")
                .OnTable("EventMetadata")
                .InSchema("EventStore")
                .WithOptions().Unique()
                .OnColumn("EventId").Ascending()
                .OnColumn("Name");
        }

        private void CreateGeneralLedgerTable()
        {
            Create.Table("GeneralLedger")
                .WithDescription("Table for holding transactions made against the general ledger")
                .InSchema("Accounting")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("CreditAmount")
                    .AsDecimal()
                    .NotNullable()
                    .WithDefaultValue(0)
                .WithColumn("DebitAmount")
                    .AsDecimal()
                    .NotNullable()
                    .WithDefaultValue(0)
                .WithColumn("LineItemId")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.GeneralLedger.LineItemId", "Accounting.LineItem.Id"),"Accounting", "LineItem", "Id")
                    .Indexed()
                .WithColumn("CreatedOn")
                    .AsDateTime()
                    .NotNullable()
                    .Indexed()
                .WithColumn("CreatedById")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.GeneralLedger.CreatedById", "Accounting.LineItem.Id"), "Users", "User", "Id");
        }

        private void CreateInvoiceTable()
        {
            Create.Table("Invoice")
                .WithDescription("Table for holding invoices")
                .InSchema("Accounting")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Balance")
                    .AsDecimal()
                    .NotNullable()
                .WithColumn("CreatedById")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.Invoice.CreatedById", "Users.User.Id"), "Users", "User", "Id")
                    .Indexed()
                .WithColumn("InvoiceNumber")
                    .AsString(20)
                    .NotNullable()
                    .Indexed()
                .WithColumn("CompanyId")
                    .AsGuid()
                    .ForeignKey(CreateForeignKeyName("Accounting.Invoice.CompanyId", "Companies.Company.Id"), "Companies", "Company", "Id")
                .WithColumn("CreatedOn")
                    .AsDateTime()
                    .NotNullable()
                    .Indexed();
        }

        private void CreateLineItemTable()
        {
            Create.Table("LineItem")
                .WithDescription("Table for holding line items that are associated with an invoice")
                .InSchema("Accounting")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("InvoiceId")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.LineItem.InvoiceId", "Accounting.Invoice.Id"), "Accounting", "Invoice", "Id")
                    .Indexed()
                .WithColumn("Description")
                    .AsString(255)
                    .NotNullable()
                .WithColumn("Amount")
                    .AsDecimal()
                    .NotNullable()
                .WithColumn("IsPaid")
                    .AsBoolean()
                    .NotNullable()
                    .WithDefaultValue(false)
                    .Indexed()
                .WithColumn("CreatedOn")
                    .AsDateTime()
                    .NotNullable()
                .WithColumn("CreatedById")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.LineItem.CreatedById", "Users.User.Id"), "Users", "User", "Id").Indexed();
        }

        private void CreateStreamTable()
        {
            Create.Table("Stream")
                .WithDescription("Contains information about the different streams in the event store")
                .InSchema("EventStore")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Name")
                    .AsString(100)
                    .NotNullable()
                    .Indexed();
        }

        private void CreateUserTable()
        {
            Create.Table("User")
                .WithDescription("Contains information about the users using the system")
                .InSchema("Users")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Email")
                    .AsString(255)
                    .NotNullable()
                    .Indexed()
                .WithColumn("FirstName")
                    .AsString(100)
                    .NotNullable()
                .WithColumn("LastName")
                    .AsString(100)
                    .NotNullable()
                    .Indexed()
                .WithColumn("CreatedOn")
                    .AsDateTime()
                    .NotNullable();
        }

        private void CreatePaymentTable()
        {
            Create.Table("Payment")
                .WithDescription("Contains information about the payments that have been entered into the system")
                .InSchema("Accounting")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("ReceivedOn")
                    .AsDateTime()
                    .NotNullable()
                    .Indexed()
                .WithColumn("ReceivedById")
                    .AsGuid()
                    .NotNullable()
                    .ForeignKey(CreateForeignKeyName("Accounting.Payment.ReceivedById", "Users.User.Id"), "Users", "User", "Id")
                    .Indexed()
                .WithColumn("Amount")
                    .AsDecimal()
                    .NotNullable()
                .WithColumn("Balance")
                    .AsDecimal()
                    .NotNullable();
        }

        private void CreateTableTable()
        {
            Create.Table("Table")
                .WithDescription("Holds information about the various tables used by the system")
                .InSchema("Metadata")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Name")
                    .AsString(255)
                    .NotNullable()
                    .Indexed();
        }

        private void CreateReportTable()
        {
            Create.Table("Report")
                .WithDescription("Table for holding information about reports in the system")
                .InSchema("Reports")
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("Name")
                    .AsString(255)
                    .NotNullable();
        }

        private void CreateSchemas()
        {
            Create.Schema("Companies");
            Create.Schema("EventStore");
            Create.Schema("Accounting");
            Create.Schema("Users");
            Create.Schema("Reports");
            Create.Schema("Metadata");
        }

        
        private static string CreateForeignKeyName(string fqSource, string fqDest) => $"FK_{fqSource}_{fqDest}";
    }
}
