using System.Net;
using Bogus;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.ffffff} - {Environment.MachineName}");

app.MapGet(
    "/contacts",
    () => new Faker<Contact>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.FirstName, f => f.Person.FirstName)
        .RuleFor(p => p.LastName, f => f.Person.LastName)
        .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName, "faker.com"))
        .RuleFor(p => p.DateOfBirth, f => f.Date.Past(100, DateTime.Now.AddYears(-18)))
        .Generate(500));

app.MapGet(
    "/check-ip",
     (HttpContext context) =>
     {
        string? ip = null;

        var header = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if(IPAddress.TryParse(header, out var remoteIpAddress))
        {
            ip += $"IP From 'X-Real-IP': {remoteIpAddress}" + Environment.NewLine;
        }

        header = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if(IPAddress.TryParse(header, out remoteIpAddress))
        {
            ip += $"IP From 'X-Forwarded-For': {remoteIpAddress}" + Environment.NewLine;
        }

        remoteIpAddress = context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        if(remoteIpAddress is not null)
        {
            ip += $"IP From 'IHttpConnectionFeature': {remoteIpAddress}" + Environment.NewLine;
        }

        remoteIpAddress = context?.Connection?.RemoteIpAddress;
        if(remoteIpAddress is not null)
        {
            ip += $"IP From 'Connection.RemoteIpAddress': {remoteIpAddress}" + Environment.NewLine;
        }

        return ip;
     });

app.Run();


public record Contact
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public string Email { get; init; } = "";
    public DateTime DateOfBirth { get; init; }
}
