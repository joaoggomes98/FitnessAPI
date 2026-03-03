using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //localhost:5001/api/members
    //[Authorize]
    public class MembersController(AppDbContext context) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.Select(user => new MemberDto
            {
                Id = user.Id,
                DisplayName = user.Name,
                Email = user.Email
            })
        .ToListAsync();

            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMembersById(int id)
        {
            var member = await context.Users.FindAsync(id);

            if (member == null) return NotFound();

            return member;
        }

    }
}
