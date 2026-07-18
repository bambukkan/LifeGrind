public class QuestEntity
{
    public Guid Id {get;set;}
    public string Title {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;

    public QuestDifficulty Difficulty {get;set;} 
    public QuestStatus Status {get;set;} = QuestStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public int ExperienceReward {get;set;}
    public decimal CoinReward {get;set;}

    public Guid UserId {get;set;}
    public UserEntity User {get;set;} = null!;

    public Guid SkillId { get; set; }
    public SkillEntity Skill { get; set; } = null!;
}

public enum QuestDifficulty
{
    Easy,
    Medium,
    Hard
}

public enum QuestStatus
{
    Active,
    Completed,
    Cancelled
}