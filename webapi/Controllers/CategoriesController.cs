using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models.Domain;
using webapi.Models.DTO;
using webapi.Repositories.Interfaces;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
    {
        var category = await categoryRepository.CreateAsync(
        new()
        {
            Name = request.Name,
            UrlHandle = request.UrlHandle,
        });

        return Ok(new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle,
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryRepository.GetAllAsync();
        return Ok(categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            UrlHandle = c.UrlHandle,
        }));
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var category = await categoryRepository.GetById(id);
        if(category == null)
        {
            return NotFound();
        }
        return Ok(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle,
        });
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
    {
        var category = new Category
        {
            Id = id,
            Name = request.Name,
            UrlHandle = request.UrlHandle,
        };
        var newCategory = await categoryRepository.UpdateAsync(category);

        if(newCategory == null) return NotFound();

        return Ok(new CategoryDto
        {
            Id = newCategory.Id,
            Name = newCategory.Name,
            UrlHandle = newCategory.UrlHandle,
        });
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var deleted = await categoryRepository.DeleteAsync(id);

        if(!deleted) return NotFound();

        return Ok();
    }

}
