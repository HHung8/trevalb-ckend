using System.Net;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Helpers;

public static class BookingHtmlBuilder
{
    public static string BuildSuccessHtml(BookingPublicInfoResult b)
    {
        bool isConfirmed = b.Status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase);
        string statusColor = isConfirmed ? "#22c55e" : "#eab308";
        string statusLabel = isConfirmed ? "Đã xác nhận" : "Chờ xử lý";

        return $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  <title>Vé tour - {WebUtility.HtmlEncode(b.BookingCode)}</title>
  <style>
    body {{ background:#121212; color:#fff; font-family:-apple-system,Segoe UI,Roboto,sans-serif; margin:0; padding:24px; display:flex; justify-content:center; }}
    .card {{ background:#1c1c1c; border-radius:20px; padding:24px; max-width:400px; width:100%; }}
    .badge {{ background:{statusColor}22; color:{statusColor}; padding:6px 14px; border-radius:999px; font-size:13px; display:inline-block; margin-bottom:16px; }}
    h1 {{ font-size:20px; margin:0 0 4px; }}
    .code {{ color:#818cf8; font-weight:700; text-align:center; font-size:22px; letter-spacing:1px; margin:12px 0 20px; }}
    img.thumb {{ width:100%; border-radius:14px; margin-bottom:16px; object-fit:cover; max-height:200px; }}
    .row {{ display:flex; justify-content:space-between; padding:10px 0; border-bottom:1px solid #2a2a2a; }}
    .row span:first-child {{ color:#888; }}
  </style>
</head>
<body>
  <div class='card'>
    <span class='badge'>{statusLabel}</span>
    {(string.IsNullOrEmpty(b.ThumbnailUrl) ? "" : $"<img class='thumb' src='{WebUtility.HtmlEncode(b.ThumbnailUrl)}' />")}
    <h1>{WebUtility.HtmlEncode(b.TourTitle)}</h1>
    <div class='code'>{WebUtility.HtmlEncode(b.BookingCode)}</div>
    <div class='row'><span>Ngày khởi hành</span><span>{b.TravelDate:dd/MM/yyyy}</span></div>
    <div class='row'><span>Số khách</span><span>{b.NumGuests} người</span></div>
    <div class='row'><span>Trạng thái</span><span>{statusLabel}</span></div>
  </div>
</body>
</html>";
    }

    public static string BuildNotFoundHtml()
    {
        return @"
<!DOCTYPE html>
<html lang='vi'>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  <title>Không tìm thấy booking</title>
  <style>
    body { background:#121212; color:#fff; font-family:-apple-system,Segoe UI,Roboto,sans-serif; margin:0; padding:24px; display:flex; justify-content:center; align-items:center; height:100vh; text-align:center; }
    .msg { color:#888; }
  </style>
</head>
<body>
  <div>
    <h2>Không tìm thấy thông tin booking</h2>
    <p class='msg'>Mã QR không hợp lệ hoặc đã bị xóa.</p>
  </div>
</body>
</html>";
    }
}