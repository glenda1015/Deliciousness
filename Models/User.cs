using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class User
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 1)] //this sets the min and max length for the first name
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 1)] //this sets the min and max length for the last name
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Minimum length is 5 characters")] //this sets the min and max length for the username and adds an error message if max isn't met
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,15}$", ErrorMessage ="Password not accepted")] //this provides user input validation
        //the password allowed will have at least 1 upper case, 1 lower case, 1 special character, 1 number, and no spaces
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password")] //the password is double checked to make sure that it matches with the pw entered
        public string ConfirmPassword { get; set; }
        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}