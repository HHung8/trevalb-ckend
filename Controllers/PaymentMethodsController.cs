using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/payment-methods")]
[Authorize]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _service;
    public PaymentMethodsController(IPaymentMethodService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var result = await _service.GetMineAsync(User.GetUserId());
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddPaymentMethodDto dto)
    {
        var result = await _service.AddAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<PaymentMethodDto>.Ok(result, "The payment method was added successfully"));
    }

    [HttpPut("{id:guid}/default")]
    public async Task<IActionResult> SetDefault(Guid id)
    {
        await _service.SetDefaultAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new {}, "Đã đặt làm mặc định"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Đã xoá thẻ"));
    }
}    