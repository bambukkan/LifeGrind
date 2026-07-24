public class UserEntity
{
    public Guid Id {get;set;}
    public string Name {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string PasswordHash {get;set;} = string.Empty;
    public int TotalExperience {get;set;} = 0;
    public decimal Coins {get;set;} = 0;
    public List<SkillEntity> Skills {get;set;} = new List<SkillEntity>();
    public List<QuestEntity> Quests {get;set;} = new List<QuestEntity>();
    public List<PersonalRewardEntity> PersonalRewards {get;set;} = new List<PersonalRewardEntity>();
}