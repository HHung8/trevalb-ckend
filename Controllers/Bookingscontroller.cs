using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Booking;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;


public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService) => _bookingService = bookingService;
    
   
    // ============================================================
    // TOUR BOOKINGS
    // ============================================================
    
    [HttpPost("tours")]
    public async Task<IActionResult> CreateTourBookingAsync([FromBody] CreateTourBookingDto dto)
    {
        var result = await _bookingService.CreateTourBookingAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<TourBookingDto>.Ok(result, "Đặt tour thành công. Vui lòng thanh toán để xác nhận"));
    }

    [HttpGet("tours")]
    public async Task<IActionResult> GetMyTourBookings()
    {
        var result = await _bookingService.GetUserTourBookingsAsync(User.GetUserId());
        return Ok(ApiResponse<IEnumerable<TourBookingDto>>.Ok(result));
    }
    
    [HttpGet("tours/{id:guid}")]
    public async Task<IActionResult> GetTourBookingById(Guid id)
    {
        var result = await _bookingService.GetTourBookingByIdAsync(id, User.GetUserId());
        return Ok(ApiResponse<TourBookingDto>.Ok(result));
    }
    
    [HttpPost("tours/{id:guid}/cancel")]
    public async Task<IActionResult> CancelTourBooking(Guid id)
    {
        await _bookingService.CancelTourBookingAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Hủy booking tour thành công."));
    }
    
    // ============================================================
    // HOTEL BOOKINGS
    // ============================================================

    [HttpPost("hotels")]
    public async Task<IActionResult> CreateHotelBooking([FromBody] CreateHotelBookingDto dto)
    {
        var result = await _bookingService.CreateHotelBookingAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<HotelBookingDto>.Ok(result, "Đặt phòng thành công. Vui lòng thanh toán để xác nhận."));
    }
    
    [HttpGet("hotels")]
    public async Task<IActionResult> GetMyHotelBookings()
    {
        var result = await _bookingService.GetUserHotelBookingsAsync(User.GetUserId());
        return Ok(ApiResponse<IEnumerable<HotelBookingDto>>.Ok(result));
    }
 
    [HttpGet("hotels/{id:guid}")]
    public async Task<IActionResult> GetHotelBookingById(Guid id)
    {
        var result = await _bookingService.GetHotelBookingByIdAsync(id, User.GetUserId());
        return Ok(ApiResponse<HotelBookingDto>.Ok(result));
    }
 
    [HttpPost("hotels/{id:guid}/cancel")]
    public async Task<IActionResult> CancelHotelBooking(Guid id)
    {
        await _bookingService.CancelHotelBookingAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Hủy booking phòng thành công."));
    }
    

}