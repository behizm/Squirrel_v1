﻿using System.Collections.Generic;
using System.Linq;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ExtensionMethods
{
    public static class TopicExtensions
    {
        public static ICollection<Post> OrderByPostOrdering(this ICollection<Post> posts, PostsOrdering postsOrdering)
        {
            switch (postsOrdering)
            {
                case PostsOrdering.LastEdited:
                    return posts.OrderByDescending(x => x.EditDate.HasValue ? x.EditDate.Value : x.CreateDate).ToList();

                case PostsOrdering.Newer:
                    return posts.OrderByDescending(x => x.CreateDate).ToList();

                case PostsOrdering.Older:
                    return posts.OrderBy(x => x.CreateDate).ToList();

                case PostsOrdering.Popular:
                    return posts.OrderByDescending(x => x.Votes.Summery()).ToList();

                default:
                    return posts.ToList();
            }
        }

        public static ICollection<Post> PublicOrderedPosts(this ICollection<Post> posts, PostsOrdering postsOrdering)
        {
            var privatePosts = posts.Where(x => !x.IsPublic).ToList();
            privatePosts.ForEach(x => posts.Remove(x));

            switch (postsOrdering)
            {
                case PostsOrdering.LastEdited:
                    return posts.OrderByDescending(x => x.EditDate.HasValue ? x.EditDate.Value : x.CreateDate).ToList();

                case PostsOrdering.Newer:
                    return posts.OrderByDescending(x => x.CreateDate).ToList();

                case PostsOrdering.Older:
                    return posts.OrderBy(x => x.CreateDate).ToList();

                case PostsOrdering.Popular:
                    return posts.OrderByDescending(x => x.Votes.Summery()).ToList();

                default:
                    return posts.ToList();
            }
        }


        public static List<Post> SortedPosts(this Topic topic)
        {
            if (topic.Posts == null || !topic.Posts.Any())
            {
                return new List<Post>();
            }

            if (topic.Posts.Count() == 1)
            {
                return topic.Posts.ToList();
            }

            switch (topic.PostsOrdering)
            {
                case PostsOrdering.LastEdited:
                    return topic.Posts.OrderByDescending(x => x.EditDate.HasValue ? x.EditDate.Value : x.CreateDate).ToList();

                case PostsOrdering.Newer:
                    return topic.Posts.OrderByDescending(x => x.CreateDate).ToList();

                case PostsOrdering.Older:
                    return topic.Posts.OrderBy(x => x.CreateDate).ToList();

                case PostsOrdering.Popular:
                    return topic.Posts.OrderByDescending(x => x.Votes.Summery()).ToList();

                case PostsOrdering.NewerPublish:
                    return topic.Posts.OrderByDescending(x => x.PublishDate.HasValue ? x.PublishDate.Value : x.CreateDate).ToList();

                case PostsOrdering.OlderPublish:
                    return topic.Posts.OrderBy(x => x.PublishDate.HasValue ? x.PublishDate.Value : x.CreateDate).ToList();

                default:
                    return topic.Posts.ToList();
            }
        }

        public static string ImageAddress(this Topic topic)
        {
            var firstPostWithImage = topic.SortedPosts().FirstOrDefault(x => x.HeaderImageId.HasValue);
            return
                firstPostWithImage == null
                    ? null
                    : firstPostWithImage.HeaderImage.Address;
        }

        public static string Abstarct(this Topic topic)
        {
            var firstPost = topic.SortedPosts().FirstOrDefault();
            return
                firstPost == null
                    ? null
                    : firstPost.Abstract;
        }

    }
}
