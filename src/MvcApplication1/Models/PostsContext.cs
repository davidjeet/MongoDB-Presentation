using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace MvcApplication1.Models
{
    public class PostsContext
    {
        private const string ConnectionString = "mongodb://localhost/?safe=true";

        public MongoDatabase Database;

        public PostsContext()
        {
            MongoClient client = new MongoClient(ConnectionString);
            MongoServer server = client.GetServer();
            Database = server.GetDatabase("test");
        }

        public MongoCollection<Post> Posts => Database.GetCollection<Post>("posts");


        ////public IMongoDatabase Database;

        ////public PostsContext()
        ////{
        ////    MongoClient client = new MongoClient(ConnectionString);
        ////    Database = client.GetDatabase("test");
        ////}

        ////public IMongoCollection<Post> Posts => Database.GetCollection<Post>("posts");
    }
}