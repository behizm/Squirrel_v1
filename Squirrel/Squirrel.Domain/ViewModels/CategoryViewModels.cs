using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
    public class CategorySearchModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Parent { get; set; }
    }
}
