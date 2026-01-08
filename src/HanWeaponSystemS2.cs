

using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;

namespace HanWeaponSystemS2;

[PluginMetadata(
    Id = "HanWeaponSystemS2",
    Version = "1.5.0",
    Name = "H-AN 武器系统/H-AN WeaponSystem for Sw2",
    Author = "H-AN",
    Description = "CS2 武器系统/CS2 WeaponSystem")]
public partial class HanWeaponSystemS2(ISwiftlyCore core) : BasePlugin(core)
{
    private ServiceProvider? ServiceProvider { get; set; }

    private HanWeaponSystemConfig _WeaponCFG = null!;
    private HanWeaponSystemHookEvent _Hooks = null!;
    private HanWeaponSystemHelpers _Helpers = null!;
    private HanWeaponSystemService _Service = null!;


    public override void Load(bool hotReload)
    {
        Core.Configuration.InitializeJsonWithModel<HanWeaponSystemConfig>("HanWeaponSystemConfig.jsonc", "HanWeaponSystemCFG").Configure(builder =>
        {
            builder.AddJsonFile("HanWeaponSystemConfig.jsonc", false, true);
        });

        var collection = new ServiceCollection();
        collection.AddSwiftly(Core);

        collection
            .AddOptionsWithValidateOnStart<HanWeaponSystemConfig>()
            .BindConfiguration("HanWeaponSystemCFG");

        collection.AddSingleton<HanWeaponSystemHookEvent>();
        collection.AddSingleton<HanWeaponSystemHelpers>();
        collection.AddSingleton<HanWeaponSystemService>();

        ServiceProvider = collection.BuildServiceProvider();
        _Hooks = ServiceProvider.GetRequiredService<HanWeaponSystemHookEvent>();
        _Helpers = ServiceProvider.GetRequiredService<HanWeaponSystemHelpers>();
        _Service = ServiceProvider.GetRequiredService<HanWeaponSystemService>();

        var CFGMonitor = ServiceProvider.GetRequiredService<IOptionsMonitor<HanWeaponSystemConfig>>();

        _WeaponCFG = CFGMonitor.CurrentValue;

        CFGMonitor.OnChange(newConfig =>
        {
            _WeaponCFG = newConfig;
            Core.Logger.LogInformation("[H-AN] 武器系统配置文件已热重载并同步。");
        });

        _Service.RegisterCommand();
        _Hooks.HookEvents();

    }

    public override void Unload()
    {
        ServiceProvider!.Dispose();
    }


 

}