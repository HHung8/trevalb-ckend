namespace TrevalApp.DTOs.Tour;

public record TourScheduleDto(
    Guid Id, DateTime StartDate, DateTime EndDate, int AvailableSlots, decimal? OverridePrice
    );