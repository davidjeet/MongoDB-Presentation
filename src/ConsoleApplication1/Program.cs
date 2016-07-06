using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace ConsoleApplication1
{
    class Program
    {
        private const string ConnectionString = "mongodb://localhost/?safe=true"; //assumes default port 27017 if none specified

        static void Main(string[] args)
        {
            Console.WriteLine("Inserting to MongoDB using v1.9x driver");
            Driver_Version_1_9x();

            Console.WriteLine("Inserting to MongoDB using v2.23 driver");
            Driver_Version_2_2_3();

            Console.ReadKey();
        }

        private static void Driver_Version_1_9x()
        {
            MongoServer server = new MongoClient(ConnectionString).GetServer();
            MongoDatabase  database = server.GetDatabase("test");
            var collection = database.GetCollection("friends");
            collection.Drop();
            collection.Insert(new BsonDocument { { "Firstname", "Jon" }, { "Lastname", "Snow" }, { "Age", 27 } });
            collection.Insert(new BsonDocument { { "Firstname", "Arya" }, { "Age", 18 }, { "IsAlive", true } });
        }

        private static void Driver_Version_2_2_3()
        {
            var client = new MongoClient(ConnectionString);
            IMongoDatabase database= client.GetDatabase("test");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("friends");
            //collection.DeleteMany(new BsonDocument { });
            collection.InsertOne(new BsonDocument { { "Firstname", "Jamie" }, { "Lastname", "Lannister" }, { "Age", 57 } });
            collection.InsertOne(new BsonDocument { { "Firstname", "Sansa" }, { "Lastname", "Stark" }, { "IsAlive", true } });
        }

    }
}
