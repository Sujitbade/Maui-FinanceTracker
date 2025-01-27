using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PersonalFinanceTracker.Services;


namespace PersonalFinanceTracker
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


            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddScoped<TagService>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<DebtService>();
            builder.Services.AddScoped<UserBalanceService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();

            builder.Services.AddMudServices();
#endif

            return builder.Build();
        }
    }
}
