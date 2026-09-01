using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Auth;
using SB.GestionSolicitudes.Application.DTOs.Dashboard;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Login_ConCredencialesInvalidas_RetornaBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequestDto
        {
            Correo = "admin@sb.gob.do",
            Password = "PasswordIncorrecto!"
        });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_Login_ConCredencialesCorrectas_RetornaJwtToken()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequestDto
        {
            Correo = "admin@sb.gob.do",
            Password = "Admin123!"
        });

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        Assert.NotNull(body);
        Assert.True(body.Success);
        Assert.False(string.IsNullOrWhiteSpace(body.Data?.Token));
    }

    [Fact]
    public async Task Get_Solicitudes_ConTokenValido_RetornaListaPaginada()
    {
        // 1. Auth Admin
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequestDto
        {
            Correo = "admin@sb.gob.do",
            Password = "Admin123!"
        });
        var loginData = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        var token = loginData!.Data!.Token;

        // 2. Add Bearer Header
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 3. Call GET /api/v1/solicitudes
        var response = await _client.GetAsync("/api/v1/solicitudes");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<PaginatedList<SolicitudDto>>>();
        Assert.NotNull(body);
        Assert.True(body.Success);
        Assert.NotEmpty(body.Data!.Items);
    }

    [Fact]
    public async Task Get_DashboardResumen_ConTokenValido_RetornaMetricas()
    {
        // 1. Auth Admin
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequestDto
        {
            Correo = "admin@sb.gob.do",
            Password = "Admin123!"
        });
        var loginData = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        var token = loginData!.Data!.Token;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 2. Call GET /api/v1/dashboard/resumen
        var response = await _client.GetAsync("/api/v1/dashboard/resumen");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardResumenDto>>();
        Assert.NotNull(body);
        Assert.True(body.Success);
        Assert.True(body.Data!.TotalSolicitudes > 0);
    }
}
