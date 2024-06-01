using Microsoft.AspNetCore.Mvc;
using webapi.Models.Domain;
using webapi.Models.DTO;
using webapi.Repositories.Interfaces;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryId in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryId);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAysnc(blogPost);

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList(),
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            return Ok(blogPosts.Select(b => new BlogPostDto
            {
                Id = b.Id,
                Author = b.Author,
                Content = b.Content,
                FeaturedImageUrl = b.FeaturedImageUrl,
                IsVisible = b.IsVisible,
                PublishedDate = b.PublishedDate,
                ShortDescription = b.ShortDescription,
                Title = b.Title,
                UrlHandle = b.UrlHandle,
                Categories = b.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList(),
            }));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if (blogPost == null) return NotFound();

            return Ok(new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList(),
            });
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryId in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryId);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            return Ok(new BlogPostDto
            {
                Id = updatedBlogPost.Id,
                Author = updatedBlogPost.Author,
                Content = updatedBlogPost.Content,
                FeaturedImageUrl = updatedBlogPost.FeaturedImageUrl,
                IsVisible = updatedBlogPost.IsVisible,
                PublishedDate = updatedBlogPost.PublishedDate,
                ShortDescription = updatedBlogPost.ShortDescription,
                Title = updatedBlogPost.Title,
                UrlHandle = updatedBlogPost.UrlHandle,
                Categories = updatedBlogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList(),
            });
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPostById([FromRoute] Guid id)
        {
            if (await blogPostRepository.DeleteAsync(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
