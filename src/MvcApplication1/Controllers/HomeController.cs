using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {

        public readonly PostsContext Context = new PostsContext();
        public readonly PostsContextNewApi ContextNewApi = new PostsContextNewApi();

        public async Task<ActionResult> Index()
        {
            var posts = await ContextNewApi.Posts
                            .Find(new BsonDocument())
                            .SortByDescending(s => s.CreatedOn)
                            .ToListAsync();
                        
            if (posts.Any())
            {
                ViewBag.TagCloud = GetTagList(ContextNewApi.Posts);
            }
            return View(posts);
        }



        [HttpGet]
        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(PostViewModel vm)
        {
            var post = vm.GetPost();
            Context.Posts.Insert(post);

            ModelState.Clear();
            ViewBag.Title = "Add New Post";
            ViewBag.Status = string.Format("*** New post {0} was created! ***", post.Id);

            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var post = Context.Posts.AsQueryable<Post>().SingleOrDefault(p => p.Id == id);
            
            
            var vm = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                FirstName = post.Author.FirstName,
                LastName = post.Author.LastName,
                Tags = (post.Tags == null || post.Tags.Count == 0)
                      ? string.Empty
                      : post.Tags.Aggregate((current, next) => current + ", " + next)
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(PostViewModel vm)
        {
            var post = vm.GetPost();
            Context.Posts.Save(post);

            ViewBag.Title = "Edit Post";
            ViewBag.Status = string.Format("*** Post {0} was updated! ***", post.Id);

            return View(vm);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var query = MongoDB.Driver.Builders.Query.EQ("_id", ObjectId.Parse(id));
            var post = Context.Posts.Remove(query);
            ViewBag.Status = string.Format("*** Post {0} was deleted! ***", id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddComment(string id)
        {
            return View(new CommentViewModel(id));
        }

        [HttpPost]
        public ActionResult AddComment(CommentViewModel vm)
        {
            var comment = vm.GetComment();
            var query = MongoDB.Driver.Builders.Query.EQ("_id", ObjectId.Parse(vm.PostId));
            var post = Context.Posts.FindOne(query);
            post.Comments.Add(comment);
            Context.Posts.Save(post);

            ViewBag.Title = "Add Comment";
            ViewBag.Status = string.Format("*** Post {0} has a new comment by {1} {2} ! ***", 
                                            vm.PostId, comment.Author.FirstName, comment.Author.LastName);

            return RedirectToAction("Index");
        }


        #region Map Reduce Tag Cloud

        public class PostTags
        {
            public List<string> Tags { get; set; }            
        }

        private string GetTagList(IMongoCollection<Post> posts)
        {

            var unwind = new BsonDocument
            {
                {
                    "$unwind", "$Tags"      
                }
            };

            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        {"_id", "$Tags"},
                        {"TagFrequency", new BsonDocument {{"$sum", 1}}}
                    }
                }   
            };

            var sort = new BsonDocument("$sort", new BsonDocument("TagFrequency", -1));


            AggregateArgs aggregateArgs = new AggregateArgs()
            {
                Pipeline = new[]
                {
                    unwind,
                    group,
                    sort
                }
            };

            var result = Context.Posts.Aggregate(aggregateArgs).ToList();


            var result2 = posts.Aggregate()
                    .Unwind(t => t.Tags)
                    .Group(new BsonDocument { { "_id", "$Tags" }, { "TagFrequency", new BsonDocument("$sum", 1) } })
                    .Sort(new BsonDocument("TagFrequency", -1))
                    .ToList();
   

            var results = new Dictionary<string, int>();
            result2.ForEach(doc =>
            {
                results.Add(doc["_id"].AsString, doc["TagFrequency"].AsInt32);
            });

            
            var list = new List<string>();

            results.Keys.ToList().ForEach(key => list.Add(
               string.Format("{0}({1})", key, results[key])));

            return string.Join(", ", list);
        }



        #endregion
    }
}
