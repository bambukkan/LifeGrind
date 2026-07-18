public record CreateQuestRequest(
    string Title,
    string Description,
    QuestDifficulty Difficulty,
    // статус нет смысла делать, ведь при создание сущнсоит он уже делается активным
    int ExperienceReward,
    decimal CoinReward

);

public record UpdateQuestRequest(
    string Title,
    string Description,
    QuestDifficulty Difficulty,
    // статус нет смысла делать, ведь при создание сущнсоит он уже делается активным
    int ExperienceReward,
    decimal CoinReward
);

public record UpdateUserFinishQuestRequest(
    QuestStatus Status
);