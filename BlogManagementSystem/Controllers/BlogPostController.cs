using BlogManagementSystem.Models;
using BlogManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<ActionResult> GetlogPost()
        {
            var blogPosts = await _blogPostService.GetAllBlogPost();
            if (blogPosts != null)
            {
                return Ok(blogPosts);
            }
            //return BadRequest(new { message = "errorOccurred" });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>>  GetBlogPostById(int id)
        {
            var blogPosts = await _blogPostService.GetBlogPostById(id);
            if (blogPosts != null)
            {
                return Ok(new { blogPosts });
            }

            return await _blogPostService.GetBlogPostById(id);

        }

        [HttpPost]
        public async Task<ActionResult<BlogPost>> AddBlogPost([FromBody] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {  
                var newPlogPost = await _blogPostService.AddBlogPost(blogPost);
                if (newPlogPost != null)
                {
                    return Ok(newPlogPost);
                }
                else
                {
                    return BadRequest(new { message = "failedOperation" });
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BlogPost>> UpdateBlogPost(int id, [FromBody] BlogPost blogPost)
        {
            if(blogPost == null)
            {
                return BadRequest("blogPost object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                var blogPostObj = await _blogPostService.GetBlogPostById(id);
                if(blogPostObj == null)
                {
                    return BadRequest("Blog Post with Id " + id + " Not Exist");
                }
                else
                {
                    blogPost.ID = id;
                    var newPlogPost = await _blogPostService.EditBlogPost(blogPost);
                    if (newPlogPost != null)
                    {
                        return Ok(newPlogPost);
                    }
                    else
                    {
                        return BadRequest("failedOperation");
                    }
                }
                
            }


        }


    }
}
