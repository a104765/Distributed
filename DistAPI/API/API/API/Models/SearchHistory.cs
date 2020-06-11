using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class SearchHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int searchID { get; set; }
        public string searchQuery { get; set; }
        public string Username { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}