using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public partial class Post
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }

        public int ID { get; set; }

        public string PostPhoto { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Required]
        [Display(Name = "Title:")]
        [StringLength(100, MinimumLength = 1)]
        public string PostTitle { get; set; }

        [Required]
        [Display(Name = "Author:")]
        [StringLength(50, MinimumLength = 1)]
        public string PostAuthor { get; set; }

        [Required]
        [Display(Name = "Write your recipe here:")]
        [StringLength(2500, MinimumLength = 10)]
        public string PostContent { get; set; }
    }
}