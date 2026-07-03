namespace TrevalApp.DTOs.Post;

public record PostMediaDto(
    Guid Id, 
    string MediaUrl, 
    string MediaType, 
    int DisplayOrder);