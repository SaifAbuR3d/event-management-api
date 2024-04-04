using EventManagement.Application.Contracts.Responses;

namespace EventManagement.Application.Abstractions.QrCode;

public interface IQrCodeGenerator
{
    Task<string> GenerateQrCodeAsync(string ticketTypeName, decimal price, string checkInCode,
        string baseUrl, CancellationToken cancellationToken);
}