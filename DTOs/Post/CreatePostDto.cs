namespace TrevalApp.DTOs.Post;

public record CreatePostDto(
    string Title, 
    string Content, 
    Guid? DestinationId, 
    string? Tags);