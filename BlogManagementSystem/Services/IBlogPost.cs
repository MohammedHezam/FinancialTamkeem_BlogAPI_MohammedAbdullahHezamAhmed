using BlogManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagementSystem.Services
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPost>> GetAllBlogPost();

        Task<BlogPost> GetBlogPostById(int id);

        Task<BlogPost> AddBlogPost(BlogPost blogPost);

        Task<BlogPost> EditBlogPost(BlogPost blogPost);

        
    }
}