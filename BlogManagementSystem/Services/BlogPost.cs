using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

//using MySqlConnector;

namespace BlogManagementSystem.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly dbContext _context;

        public BlogPostService(dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPost()
        {
            return await _context.BlogPost.ToListAsync();
        }

        public async Task<BlogPost> GetBlogPostById(int id)
        {
            return await _context.BlogPost.Where(t => t.ID == id).FirstOrDefaultAsync();
        }
      
        public async Task<BlogPost> AddBlogPost(BlogPost blogPost)
        {
            
            _context.BlogPost.Add(blogPost);
            await _context.SaveChangesAsync();

            return blogPost;
        }


        public async Task<BlogPost> EditBlogPost(BlogPost blogPost)
        {
            _context.Entry(blogPost).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return blogPost;

           
        }



    }
}