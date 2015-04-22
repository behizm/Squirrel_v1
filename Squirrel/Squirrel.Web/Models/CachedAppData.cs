using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;
using Squirrel.Service;
using Squirrel.Utility.Async;

namespace Squirrel.Web.Models
{
    public static class CachedAppData
    {
        public static CacheItem<Topic> LastPublishedTopics { get; set; }
        public static CacheItem<Topic> PopularPublishedTopics { get; set; }
        public static CacheItem<CategoryNode> PublishedMainCategories { get; set; }
        public static CacheItem<TagWeightModel> PublishedTags { get; set; }
    }

    public class CacheItem<T>
    {
        public T Item { get; set; }
        public List<T> Items { get; set; }
        public DateTime? LastUpdate { get; set; }
    }

    public class CachedAppDataMethods
    {
        private ITopicService _topicService;
        private ITopicService TopicService
        {
            get { return _topicService ?? (_topicService = ServiceIOC.Get<ITopicService>()); }
        }

        private ITopicService _topicService2;
        private ITopicService TopicService2
        {
            get { return _topicService2 ?? (_topicService2 = ServiceIOC.Get<ITopicService>()); }
        }

        private ICategoryService _categoryService;
        private ICategoryService CategoryService
        {
            get { return _categoryService ?? (_categoryService = ServiceIOC.Get<ICategoryService>()); }
        }

        private ITagService _tagService;
        private ITagService TagService
        {
            get { return _tagService ?? (_tagService = ServiceIOC.Get<ITagService>()); }
        }

        // ---

        public async Task SyncLastPublishedTopicsAsync()
        {
            var searchModel = new TopicSearchModel
            {
                IsPublished = true,
                PublishDateTo = DateTime.Now,
            };
            var orderingModel = new OrderingModel<Topic, DateTime?>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.PublishDate,
                Skip = 0,
                Take = 20,
            };
            var topic = await TopicService.SearchAsync(searchModel, orderingModel);
            if (topic == null)
            {
                return;
            }

            CachedAppData.LastPublishedTopics = new CacheItem<Topic>
            {
                Item = null,
                Items = topic,
                LastUpdate = DateTime.Now,
            };
        }

        public async Task SyncPopularPublishedTopicsAsync()
        {
            var searchModel = new TopicSearchModel
            {
                IsPublished = true,
                PublishDateTo = DateTime.Now,
            };
            var orderingModel = new OrderingModel<Topic, int>
            {
                IsAscending = false,
                OrderByKeySelector = x => x.View,
                Skip = 0,
                Take = 10,
            };
            var topic = await TopicService2.SearchAsync(searchModel, orderingModel);
            if (topic == null)
            {
                return;
            }

            CachedAppData.PopularPublishedTopics = new CacheItem<Topic>
            {
                Item = null,
                Items = topic,
                LastUpdate = DateTime.Now,
            };
        }

        public async Task SyncPublishedMainCategoriesAsync()
        {
            var categoriesTree = await CategoryService.FamilyTreeAsync(true);
            var mainCategories =
                categoriesTree.Where(x => x.Node.TopicCount > 0 || x.Node.ChildTopicCount > 0)
                    .Select(x => x.Node)
                    .ToList();

            CachedAppData.PublishedMainCategories = new CacheItem<CategoryNode>
            {
                Items = mainCategories,
                Item = null,
                LastUpdate = DateTime.Now,
            };
        }

        public async Task SyncPublishedTagsAsync()
        {
            var orderingModel = new OrderingModel<TagWeightModel, int>
            {
                IsAscending = false,
                OrderByKeySelectorFunc = x => x.Weight,
                Skip = 0,
                Take = 50,
            };
            var tags = await TagService.TagsWithWeightAsync(orderingModel, true);

            CachedAppData.PublishedTags = new CacheItem<TagWeightModel>
            {
                Items = tags.List,
                Item = null,
                LastUpdate = DateTime.Now,
            };
        }

        public async Task SyncAllAsync()
        {
            var publishedTopicsTask = SyncLastPublishedTopicsAsync();
            var popularTopicsTask = SyncPopularPublishedTopicsAsync();
            var mainCategoriesTask = SyncPublishedMainCategoriesAsync();
            var publishedTagsTask = SyncPublishedTagsAsync();
            
            await mainCategoriesTask;
            await publishedTopicsTask;
            await popularTopicsTask;
            await publishedTagsTask;
        }

        public void SyncAll()
        {
            AsyncTools.ConvertToSync(SyncAllAsync);
        }

    }

}