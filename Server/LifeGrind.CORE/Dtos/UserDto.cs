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

public record LoginUserRequest(
    string Email,
    string Password
);

public record UserResponse(
    string Name,
    string Email,
    int TotalExperience,
    decimal Coins
);