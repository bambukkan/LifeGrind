public class SkillEntity
{
    public Guid Id {get;set;}
    public string Name {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public int Experience {get;set;}
    public Guid UserId {get;set;}
    public UserEntity User {get;set;} = null!;
    public List<QuestEntity> Quests {get;set;} = new List<QuestEntity>();
}