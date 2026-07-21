public record CreateSkillRequest(
    string Name,
    string Description,
    int Experience
);

public record UpdateSkillRequest(
    string Name,
    string Description,
    int Experience
);

public record SkillResponse(
    Guid Id,
    string Name,
    string Description,
    int Experience,
    int Level,
    int ExperienceForNextLevel
);