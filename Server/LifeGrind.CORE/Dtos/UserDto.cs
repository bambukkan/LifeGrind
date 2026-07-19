public record CreateUserRequest(
    string Name,
    string Email,
    string Password
);

public record UpdateUserRequest(
    string Name,
    string Email,
    string oldPassword,
    string newPassword
);

public record UpdateUserExpAndCoinsRequest(
    int TotalExperience,
    decimal Coins
);

public record LoginUserRequest(
    string Email,
    string Password
);