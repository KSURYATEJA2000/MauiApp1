using CurrieTechnologies.Razor.SweetAlert2;
using FluentValidation;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.Services.ApiCallServices;
using MauiApp1.Services.ApplicationConfigurations;
using MauiApp1.Services.SweetAlertService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Services.Configure<AppConfig>(config => builder.Configuration.GetSection("AppConfig").Bind(config));


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddScoped<AppState>();
            builder.Services.AddTransient<IValidator<ItemsModel>, ItemsModelValidator>();
            builder.Services.AddScoped<IApiCall, ApiCall>();
            builder.Services.AddScoped<CartState>();

            builder.Services.AddScoped<ISweetAlertServices, SweetAlertServices>();
            builder.Services.AddMudServices();
            builder.Services.AddSweetAlert2(options =>
            {
                options.Theme = SweetAlertTheme.Bulma;
                options.DefaultOptions = new()
                {
                    AllowEnterKey = false,
                    AllowEscapeKey = false,
                    AllowOutsideClick = false,
                    HeightAuto = false,
                    Grow = SweetAlertGrowDirection.False
                };
            });

            return builder.Build();
        }
    }
}
