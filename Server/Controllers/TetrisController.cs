using Gejms.Server.Data;
using Gejms.Server.Entities;
using Gejms.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace Gejms.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TetrisController : ControllerBase
{
    private readonly GejmsDbContext dbContext;
    private readonly UserManager<User> userManager;

    public TetrisController(GejmsDbContext dbContext, UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
    }

    [Route("SendScore")]
    [HttpPost]
    public async Task<IActionResult> SendScore(TetrisScoreDTO score)
    {
        var currentUsername = HttpContext.User?.Identity?.Name;
        var user = await userManager.FindByNameAsync(currentUsername);

        if (user == null)
            return BadRequest();

        int scoreMin = 100 * (int)Math.Pow(score.Level - 1, 2);
        int scoreMax = 100 * (int)Math.Pow(score.Level, 2);

        bool wrongScore = score.Score < scoreMin || score.Score > scoreMax;
        bool incorrectBrower = !string.Join(",", Request.Headers[HeaderNames.UserAgent]).Contains("Chrome");

        if (wrongScore || incorrectBrower)
        {
            await dbContext.Cheaters.AddAsync(new CheatingTries
            {
                User = user,
                Score = score.Score,
                Time = DateTime.UtcNow
            });
            return BadRequest();
        }

        if (score.Score == 0)
            return Ok();
        await dbContext.TetrisScores.AddAsync(new TetrisScore
        {
            User = user,
            Score = score.Score,
            Time = score.Time,
            Level = score.Level,
            ScoreDate = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [Route("myBest")]
    [HttpGet]
    public async Task<ActionResult<int>> Get()
    {
        var currentUsername = HttpContext.User?.Identity?.Name;
        var user = await userManager.FindByNameAsync(currentUsername);

        var userScores = dbContext.TetrisScores
            .AsNoTracking()
            .Where(s => s.User == user)
            .OrderByDescending(s => s.Score)
            .Take(1)
            .ToList();
        var score = userScores.Count == 0 ? 0 : userScores[0].Score;
        return Ok(score);
    }

    [Route("top")]
    [HttpGet]
    public ActionResult<List<TetrisScoreUserDTO>> GetTopFive()
    {
        var scores = dbContext.TetrisScores
            .Include(sc => sc.User).AsNoTracking()
            .OrderByDescending(s => s.Score)
            .Take(5)
            .ToList();

        var result = new List<TetrisScoreUserDTO>();
        foreach (var score in scores)
        {
            result.Add(new TetrisScoreUserDTO
            {
                Username = score.User.UserName,
                Score = score.Score
            });
        }
        return Ok(result);
    }
}
