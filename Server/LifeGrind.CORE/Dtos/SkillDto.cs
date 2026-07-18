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
