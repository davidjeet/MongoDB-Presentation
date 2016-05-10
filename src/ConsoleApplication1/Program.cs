using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsoleApplication1
{
    class Program
    {
        private const string ConnectionString = "mongodb://localhost/?safe=true";

        static void Main(string[] args)
        {
            Driver_Version_1_9x();
            Driver_Version_2_2_3();
        }

        private static void Driver_Version_1_9x()
        {
            MongoServer server = new MongoClient(ConnectionString).GetServer();
            MongoDatabase  database = server.GetDatabase("test");
            var collection = database.GetCollection("unicorns");
            collection.Drop();
            collection.Insert(new BsonDocument { { "Firstname", "Jon" }, { "Lastname", "Snow" }, { "Age", 27 } });
            collection.Insert(new BsonDocument { { "Firstname", "Arya" }, { "Age", 18 }, { "IsAlive", true } });
        }

        private static void Driver_Version_2_2_3()
        {
            var client = new MongoClient(ConnectionString);
            IMongoDatabase database= client.GetDatabase("test");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("unicorns");
            collection.DeleteMany(new BsonDocument { });
            collection.InsertOne(new BsonDocument { { "Firstname", "Keanu" }, { "Lastname", "Reeves" }, { "Age", 57 } });
            collection.InsertOne(new BsonDocument { { "Firstname", "Sansa" }, { "Lastname", "Stark" }, { "IsAlive", true } });
        }

    }
}
