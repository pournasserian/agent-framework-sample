using AgentsWebUI.Components;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddSingleton<OpenRouterClientFactory>(sp =>
{
    var apiKey = builder.Configuration["OPEN_ROUTER_API_KEY"] ?? throw new InvalidOperationException("OPEN_ROUTER_API_KEY is not set in.");
    return new OpenRouterClientFactory(apiKey!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
