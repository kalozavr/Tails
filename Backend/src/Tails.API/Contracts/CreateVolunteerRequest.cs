namespace Tails.API.Contracts;

public record CreateVolunteerRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    int ExperienceYears);