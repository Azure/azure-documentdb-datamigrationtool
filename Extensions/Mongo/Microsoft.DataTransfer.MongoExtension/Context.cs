using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Microsoft.DataTransfer.MongoExtension;
public class Context
{
    private readonly IMongoDatabase database = null!;

    public Context(string connectionString, string databaseName)
    {
        var mongoConnectionUrl = new MongoUrl(connectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
        mongoClientSettings.ClusterConfigurator = cb => {
            cb.Subscribe<CommandStartedEvent>(e => {
                System.Diagnostics.Debug.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            });
        };
        var client = new MongoClient(mongoClientSettings);
        database = client.GetDatabase(databaseName);
    }

    public virtual IRepository<T> GetRepository<T>(string name)
    {
        return new MongoRepository<T>(database.GetCollection<T>(name));
    }

    public virtual IMongoCollection<T> GetCollection<T>(string name)
    {
        return database.GetCollection<T>(name);
    }

    public virtual async Task RenameCollectionAsync(string originalName, string newName)
    {
        await database.RenameCollectionAsync(originalName, newName);
    }

    public virtual async Task DropCollectionAsync(string name)
    {
        await database.DropCollectionAsync(name);
    }

    public virtual async Task<IList<string>> GetCollectionNamesAsync()
    {
        var names = await database.ListCollectionNamesAsync();
        return names.ToList();
    }
}
