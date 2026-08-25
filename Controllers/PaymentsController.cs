using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;
      [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
    {
        var result = await _paymentService.CreateAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<PaymentDto>.Ok(result, "Tạo giao dịch thanh toán thành công."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _paymentService.GetByIdAsync(id, User.GetUserId());
        return Ok(ApiResponse<PaymentDetailDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyPayments([FromQuery] PaymentQueryDto query)
    {
        var result = await _paymentService.GetMyPaymentsAsync(User.GetUserId(), query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("bookings/{bookingType}/{bookingId:guid}")]
    public async Task<IActionResult> GetByBooking(string bookingType, Guid bookingId)
    {
        var result = await _paymentService.GetByBookingAsync(bookingType, bookingId);
        return Ok(ApiResponse<PaymentDto?>.Ok(result));
    }

    // Endpoint dành cho cổng thanh toán callback (webhook) — nên xác thực bằng signature riêng
    // thay vì [Authorize] user thường, tùy theo cổng thanh toán bạn tích hợp (VD: VNPay, Momo, Stripe).
    [AllowAnonymous]
    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, [FromBody] ConfirmPaymentDto dto)
    {
        var result = await _paymentService.ConfirmAsync(id, dto);
        return Ok(ApiResponse<PaymentDto>.Ok(result, "Thanh toán thành công."));
    }

    [AllowAnonymous]
    [HttpPost("{id:guid}/fail")]
    public async Task<IActionResult> Fail(Guid id, [FromBody] FailPaymentDto dto)
    {
        await _paymentService.FailAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { }, "Đã ghi nhận thanh toán thất bại."));
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id:guid}/refund")]
    public async Task<IActionResult> Refund(Guid id)
    {
        await _paymentService.RefundAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "Hoàn tiền thành công."));
    }
}