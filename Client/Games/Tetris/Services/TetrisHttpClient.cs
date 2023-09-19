using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using Gejms.Shared.Models;
using System.Text;
using System.Text.Json;

namespace Gejms.Client.Games.Tetris.Services;

public class TetrisHttpClient : IGameService
{
    private readonly HttpClient httpClient;

    public TetrisHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async ValueTask SendScore()
    {
        var scoreService = GameServicesCollection.Instance.Get<ScoreService>();
        var gameTimeService = GameServicesCollection.Instance.Get<GameTime>();
        var scoreDTO = new TetrisScoreDTO
        {
            Level = scoreService.Level,
            Score = scoreService.Score,
            Time = gameTimeService.TotalMillis
        };
        var jsonContent = JsonSerializer.Serialize(scoreDTO);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/Tetris/SendScore", stringContent);
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("sending the score to backend. response: " + json);
    }

    public async ValueTask<int> GetMyBest()
    {
        var myBestResult = await httpClient.GetAsync("/api/Tetris/myBest");
        myBestResult.EnsureSuccessStatusCode();

        var bestResult = await myBestResult.Content.ReadAsStringAsync();
        return int.Parse(bestResult);
    }

    public async ValueTask<List<TetrisScoreUserDTO>> GetTopFive()
    {
        var topResults = await httpClient.GetAsync("/api/Tetris/top");
        topResults.EnsureSuccessStatusCode();

        var top = await topResults.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TetrisScoreUserDTO>>(top, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
