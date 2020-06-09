using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProfilePreference
    {

        //Pref ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //
        [Required]
        public string Username { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        [Required]
        public bool Birthday { get; set; }

        [Required]
        public bool Email { get; set; }
    }
}