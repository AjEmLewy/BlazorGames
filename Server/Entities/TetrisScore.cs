namespace Gejms.Server.Entities;

public class TetrisScore
{
    public int ID { get; set; }
    public User User { get; set; }
    public int Score { get; set; }
    public float Time { get; set; }
    public DateTime ScoreDate { get; set; }
    public int Level { get; set; }
}
