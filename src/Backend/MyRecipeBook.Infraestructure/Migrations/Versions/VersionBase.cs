using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace MyMusicBook.Infraestructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            // Define nome, tipo, PK e que ela deve se incrementar.
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()

            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable();
    }
}
