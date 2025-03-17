using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private static readonly List<Blog> Blogs = new List<Blog>();
    private int GetNextId() => Blogs.Count == 0 ? 1 : Blogs.Max(b => b.Id) + 1;

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserCredentials credentials)
    {
        if (credentials.Username == "admin" && credentials.Password == "password")
        {
            var token = GenerateJwtToken();
            return Ok(new { token });
        }
        return Unauthorized();
    }

    [HttpGet]
    public IActionResult GetBlogs() => Ok(Blogs);

    [HttpPost]
    [Authorize]
    public IActionResult CreateBlog([FromBody] Blog blog)
    {
        blog.Id = GetNextId();
        Blogs.Add(blog);
        return CreatedAtAction(nameof(GetBlogs), new { id = blog.Id }, blog);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateBlog(int id, [FromBody] Blog updatedBlog)
    {
        var blog = Blogs.FirstOrDefault(b => b.Id == id);
        if (blog == null) return NotFound();

        blog.Title = updatedBlog.Title;
        blog.Body = updatedBlog.Body;
        return Ok(blog);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteBlog(int id)
    {
        var blog = Blogs.FirstOrDefault(b => b.Id == id);
        if (blog == null) return NotFound();

        Blogs.Remove(blog);
        return Ok();
    }

    private string GenerateJwtToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey123"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "YourIssuer",
            audience: "YourAudience",
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
