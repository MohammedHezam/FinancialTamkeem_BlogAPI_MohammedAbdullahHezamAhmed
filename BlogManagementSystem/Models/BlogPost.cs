using System.ComponentModel.DataAnnotations;

namespace BlogManagementSystem.Models
{
    public class BlogPost
    {
        public int ID { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage = "Title Is Mandatory And MustBe Not Empty")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content Is Mandatory And MustBe Not Empty")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
