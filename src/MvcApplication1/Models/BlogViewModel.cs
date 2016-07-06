using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    #region PostViewModel Implementation

    public class PostViewModel
    {
        public string Id { get; set; }

        [System.ComponentModel.DisplayName("Title of Post")]
        [Required(ErrorMessage = "You need to enter a title.")]
        public string Title { get; set; }

        [System.ComponentModel.DisplayName("Content of Post")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Tags { get; set; }

        public Post GetPost()
        {
            return new Post
            {
                Id = this.Id,
                Author = new Author { FirstName = this.FirstName, LastName = this.LastName },
                Title = this.Title,
                Content = this.Content,
                Tags = String.IsNullOrEmpty(this.Tags) ? new List<string>() : new List<string>(this.Tags.Split(new [] { ',' })),
                Comments = new List<Comment>(),
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now,
            };
        }
    }

    #endregion

    #region CommentViewModel implementation
    public class CommentViewModel
    {
        [System.ComponentModel.DisplayName("Content of Comment")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string PostId { get; set; }

        public CommentViewModel()
        {
        }

        public CommentViewModel(string postId)
        {
            this.PostId = postId;
        }

        public Comment GetComment()
        {
            return new Comment
            {
                Author = new Author
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName
                },
                Content = this.Content,
                CreatedOn = DateTime.UtcNow,
            };
        }

    }
    #endregion

}