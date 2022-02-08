// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Console;
using VendingMachine.Console.WebClient;
using VendingMachine.Console.Services.ProductContext;
using VendingMachine.Console.Services.PaymentContext;
using VendingMachine.Console.VendingMachine;
using VendingMachine.Console.CommandLine.CommandComposite;
using VendingMachine.Console.CommandLine;
using VendingMachine.Console.CommandLine.Commands.Interfaces;
using VendingMachine.Console.CommandLine.Commands;
using System.Globalization;
using VendingMachine.Console.Model;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((services) =>
{
    // infrastructure services
    services.AddSingleton<Machine>();
    services.AddSingleton<IVendingMachineFactory, VendingMachineFactory>();
    services.AddTransient<IVendingMachineInstance, VendingMachineInstance>();

    services.AddSingleton<IWallet, Wallet>();
    services.AddSingleton<IChange, Change>();

    // api services
    services.AddSingleton(typeof(IHttpRESTClient<>), typeof(HttpRESTClient<>));
    services.AddScoped   <IPaymentService, PaymentService>();
    services.AddScoped   <IProductService, ProductService>();
    services.AddScoped   <IProductStockService, ProductStockService>();

    services.AddSingleton<IOptionsSingleton, OptionsSingleton>();

    // commands
    services.AddSingleton<ICommandFactory, CommandFactory>();

    services.AddTransient<IWelcomeCommand, WelcomeCommand>();
    services.AddTransient<IInsertCoinCommand, InsertCoinCommand>();
    services.AddTransient<IShowProductCommand, ShowProductCommand>();
    services.AddTransient<IMakeChangeCommand, MakeChangeCommand>();
    services.AddTransient<IReturnMoneyCommand, ReturnMoneyCommand>();
    services.AddTransient<IFinalizeCommand, FinalizeCommand>();
    services.AddLocalization(options =>
    {
        options.ResourcesPath = "";
    });
});

CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
var host           = builder.Build();
var vendingMachine = host.Services.GetRequiredService<Machine>();
vendingMachine.Run(args);

