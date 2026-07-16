namespace TrevalApp.DTOs.Tour;

public record TourScheduleDto(
        Guid Id, 
        Guid TourId,
        DateTime StartDate, 
        DateTime EndDate, 
        int AvailableSlots, 
        decimal? OverridePrice
    );
    
public record CreateTourScheduleDto(
        DateTime StartDate,
        DateTime EndDate,
        int AvailableSlots,
        decimal? OverridePrice
    );