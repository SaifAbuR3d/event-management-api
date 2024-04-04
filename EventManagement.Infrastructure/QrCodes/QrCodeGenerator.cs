using EventManagement.Application.Abstractions.Images;
using EventManagement.Application.Abstractions.QrCode;
using QRCoder;

namespace EventManagement.Infrastructure.QrCodes;

public class QrCodeGenerator(IImageHandler imageHandler) : IQrCodeGenerator
{
    private static readonly string qrCodeImageExtension = ".png";
    public async Task<string> GenerateQrCodeAsync(string ticketTypeName, decimal price, string checkInCode,
        string baseUrl, CancellationToken cancellationToken)
    {
        var payload = $"Ticket Type: {ticketTypeName}\n" +
              $"Price: {price}\n" +
              $"CheckInCode: {checkInCode}";

        QRCodeGenerator qrGenerator = new();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(qrCodeData);
        byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

        var imageName = checkInCode;

        var directory = Path.Combine(baseUrl, "images", "qr-codes");

        var imagePath = await imageHandler.UploadQrCodeImageAsync(qrCodeAsPngByteArr, directory,
            imageName, qrCodeImageExtension, cancellationToken);

        return imagePath;

    }
}
