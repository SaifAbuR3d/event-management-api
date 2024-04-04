using EventManagement.Application.Abstractions.QrCode;
using EventManagement.Infrastructure.QrCodes;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.QrCode;

public static class QrCodeConfiguration
{
    public static IServiceCollection AddQrCodeInfrastructure(
        this IServiceCollection services)
        => services
            .AddScoped<IQrCodeGenerator, QrCodeGenerator>();

}
