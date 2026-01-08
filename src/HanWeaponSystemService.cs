using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;

namespace HanWeaponSystemS2;
public class HanWeaponSystemService
{
    private readonly ILogger<HanWeaponSystemService> _logger;
    private readonly ISwiftlyCore _core;
    private readonly IOptionsMonitor<HanWeaponSystemConfig> _weaponCfg;
    private readonly HanWeaponSystemHelpers _helpers;

    public HanWeaponSystemService(ISwiftlyCore core, ILogger<HanWeaponSystemService> logger,
        IOptionsMonitor<HanWeaponSystemConfig> weaponCfg,
        HanWeaponSystemHelpers helpers)
    {
        _core = core;
        _logger = logger;
        _weaponCfg = weaponCfg;
        _helpers = helpers;
    }


    public void RegisterCommand()
    {
        var DataConfig = _weaponCfg.CurrentValue;
        foreach (var datas in DataConfig.WeaponsList)
        {
            _core.Command.RegisterCommand(datas.Command, (context) =>
            {
                var player = context.Sender;
                if (player == null || !player.IsValid)
                    return;

                var Controller = player.Controller;
                if (Controller == null || !Controller.IsValid)
                    return;

                if(!Controller.PawnIsAlive)
                    return;

                _helpers.GiveWeaponAndSkin(player, datas);

            }, true);

        }

    }

}