public record UpdateUserRequest(
    string FullName,
    string? Email = null
);