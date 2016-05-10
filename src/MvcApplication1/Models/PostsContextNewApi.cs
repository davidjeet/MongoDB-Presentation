using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace MvcApplication1.Models
{
    public class PostsContextNewApi
    {
        private const string ConnectionString = "mongodb://localhost/?safe=true";

        public IMongoDatabase Database;

        public PostsContextNewApi()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
            settings.ClusterConfigurator = builder => builder.Subscribe<CommandStartedEvent>(started =>
            {

            }
            );
            MongoClient client = new MongoClient(settings);
            Database = client.GetDatabase("test");
        }

        public IMongoCollection<Post> Posts => Database.GetCollection<Post>("posts");
    }
}