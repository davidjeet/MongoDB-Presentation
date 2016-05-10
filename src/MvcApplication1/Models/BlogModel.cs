using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MvcApplication1.Models
{
    public class Post    
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string Title { get; set; }
        public string Content { get; set; }
        public Author Author { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public List<Comment> Comments { get; set; }
        public List<string> Tags { get; set; }

        public Post() { } //required for MongoDB auto-serialization
    }

    public class Comment
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Author Author { get; set; }

        public Comment() { } //required for MongoDB auto-serialization
    }

    public class Author
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Author() { } //required for MongoDB auto-serialization
    }
}