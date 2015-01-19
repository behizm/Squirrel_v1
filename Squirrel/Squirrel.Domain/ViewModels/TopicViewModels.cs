using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
    public class TopicAddModel
    {
        public string Title { get; set; }
        public PostsOrdering PostsOrdering { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }

    public class TopicEditModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public PostsOrdering PostsOrdering { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class TopicDeleteModel
    {
    }

    public class TopicSearchModel
    {
    }
}
