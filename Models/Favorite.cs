using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Favorite
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FavID { get; set; }

        public int ID { get; set; }

        public int FavPostID { get; set; }

        public IEnumerable<Post> PostViewModel { get; set; }

    }
}