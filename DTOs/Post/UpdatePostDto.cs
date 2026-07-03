namespace TrevalApp.DTOs.Post;

public record UpdatePostDto(
    string? Title, 
    string? Content, 
    Guid? DestinationId, 
    string? Tags, 
    bool? IsPublished);