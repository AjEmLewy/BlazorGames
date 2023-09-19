namespace Gejms.Server.Entities;

public class CheatingTries
{
    public int ID{ get; set; }
    public User User { get; set; }

    public DateTime Time { get; set; }
    public int Score { get; set; }
}
