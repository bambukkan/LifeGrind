public record UpdatePersonalRewardRequest(
    string Name,
    string Description,
    decimal Cost
);

public record CreatePersonalRewardRequest(
    string Name,
    string Description,
    decimal Cost
);

public record PersonalRewardResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Cost
);