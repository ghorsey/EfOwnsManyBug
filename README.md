# Example of a bug in EF Core
There is an issue with owns many and deleting the parent where the owned collection attempts to be re-inserted.
This scenario throws the following exception:
```shell
Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the FOREIGN KEY constraint "FK_PostSummaryTag_PostSummary_PostId". The conflict occurred in database "...\EFOWNSMANYBUG\SRC\EFOWNSMANYBUG\DATA\SAMPLEDATABASE.MDF", table "dbo.PostSummary", column 'Id'.
```

## Setup
1. In the root folder run `dotnet tool restore`.
2. Create a SQL based database. (On my local setup I created `Data\SampleDatabase.mdf` and used a connection string with the full path to the \*.mdf file)
3. Store the connection string in a secret named `ConnectionString` so the `BloggingDbContext` can have access to the database.3
4. Execute `dotnet ef -p .\src\EfOwnsManyBug\ database update` to seed the database with the test data.
5. Exectue `dotnet run --project .\src\EfOwnsManyBug\` to experience the bug.