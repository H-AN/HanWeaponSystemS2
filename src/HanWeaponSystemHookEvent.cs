using System.Security.AccessControl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Cecil.Cil;
using Spectre.Console;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Helpers;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.ProtobufDefinitions;
using SwiftlyS2.Shared.SchemaDefinitions;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static HanWeaponSystemS2.HanWeaponSystemConfig;
using static HanWeaponSystemS2.HanWeaponSystemHelpers;

namespace HanWeaponSystemS2;
public class HanWeaponSystemHookEvent
{
    private readonly ILogger<HanWeaponSystemHookEvent> _logger;
    private readonly ISwiftlyCore _core;
    private readonly IOptionsMonitor<HanWeaponSystemConfig> _weaponCfg;
    private readonly HanWeaponSystemHelpers _helpers;

    public HanWeaponSystemHookEvent(ISwiftlyCore core, ILogger<HanWeaponSystemHookEvent> logger,
        IOptionsMonitor<HanWeaponSystemConfig> weaponCfg,
        HanWeaponSystemHelpers helpers)
    {
        _core = core;
        _logger = logger;
        _weaponCfg = weaponCfg;
        _helpers = helpers;
    }

    public void HookEvents()
    {
        _core.GameEvent.HookPre<EventPlayerDeath>(OnPlayerDeath);
        _core.Event.OnPrecacheResource += Event_OnPrecacheResource;
        _core.Event.OnTick += Event_OnTick;
        _core.Event.OnEntityTakeDamage += Event_OnEntityTakeDamage;
        _core.GameEvent.HookPre<EventPlayerHurt>(OnPlayerHurt);
    }

    private void Event_OnTick()
    {
        foreach (var player in _core.PlayerManager.GetAllPlayers())
        {
            if (player == null || !player.IsValid)
                continue;

            var pawn = player.PlayerPawn;
            if (pawn == null || !pawn.IsValid)
                continue;

            var controller = player.Controller;
            if (controller == null || !controller.IsValid)
                continue;

            var ControllerValue = controller.PlayerPawn.Value;
            if (ControllerValue == null || !ControllerValue.IsValid)
                continue;

            var WeaponServices = ControllerValue.WeaponServices;
            if (WeaponServices == null || !WeaponServices.IsValid)
                continue;

            var weapon = WeaponServices.ActiveWeapon.Value;
            if (weapon == null || !weapon.IsValid)
                continue;

            var classname = weapon.DesignerName;
            var customName = weapon.AttributeManager.Item.CustomName;

            bool isCustom = !string.IsNullOrEmpty(customName);

            var dataConfig = _weaponCfg.CurrentValue;
            var weaponData = dataConfig.WeaponsList.FirstOrDefault(w =>
                w.ClassName.Equals(classname, StringComparison.OrdinalIgnoreCase)
                && (isCustom ? w.CustomName == customName : string.IsNullOrEmpty(w.CustomName))
            );

            if (weaponData != null)
            {
                if (weaponData.NoRecoil)
                {
                    pawn.AimPunchAngle.Pitch = 0;
                    pawn.AimPunchAngle.Yaw = 0;
                    pawn.AimPunchAngle.Roll= 0;
                    pawn.AimPunchAngleVel.Pitch = 0;
                    pawn.AimPunchAngleVel.Yaw = 0;
                    pawn.AimPunchAngleVel.Roll = 0;
                    pawn.AimPunchTickFraction = 0;
                }
            }


        }

    }

    private void Event_OnPrecacheResource(SwiftlyS2.Shared.Events.IOnPrecacheResourceEvent @event)
    {
        var List = _weaponCfg.CurrentValue.WeaponsList;
        foreach (var weapon in List)
        {
            if (!string.IsNullOrEmpty(weapon.PrecacheModel))
            {
                @event.AddItem(weapon.PrecacheModel);
                Console.WriteLine($"PrecacheMod: {weapon.PrecacheModel}");
            }
        }
        foreach (var sound in List)
        {
            if (!string.IsNullOrEmpty(sound.PrecacheSoundEvent))
            {
                @event.AddItem(sound.PrecacheSoundEvent);
                Console.WriteLine($"PrecacheMod: {sound.PrecacheSoundEvent}");
            }
        }
    }
    private HookResult OnPlayerDeath(EventPlayerDeath @event)
    {
        var attackerId = @event.Attacker;

        var attacker = _core.PlayerManager.GetPlayer(attackerId);
        if (attacker == null || !attacker.IsValid)
            return HookResult.Continue;

        var attackerpawn = attacker.PlayerPawn;
        if (attackerpawn == null || !attackerpawn.IsValid)
            return HookResult.Continue;

        var activeWeapon = attackerpawn.WeaponServices?.ActiveWeapon.Value;
        if (activeWeapon == null || !activeWeapon.IsValid)
            return HookResult.Continue;

        string customName = activeWeapon.AttributeManager.Item.CustomName;
        if (string.IsNullOrEmpty(customName))
            return HookResult.Continue;

        var dataConfig = _weaponCfg.CurrentValue;

        var weaponConfig = dataConfig.WeaponsList.FirstOrDefault(w => w.CustomName == customName);
        if (weaponConfig == null)
            return HookResult.Continue;

        if (!string.IsNullOrEmpty(weaponConfig.KillIcon))
        {
            @event.Weapon = weaponConfig.KillIcon;
        }
        else
        {
            @event.Weapon = weaponConfig.ClassName;
        }

        return HookResult.Continue;
    }

    private void Event_OnEntityTakeDamage(IOnEntityTakeDamageEvent @event)
    {
        var victim = @event.Entity;
        if (victim == null || !victim.IsValid)
            return;

        var VictimPawn = victim.As<CCSPlayerPawn>();
        if (VictimPawn == null || !VictimPawn.IsValid)
            return;
        

        var attacker = @event.Info.Attacker.Value;
        if (attacker == null || !attacker.IsValid)
            return;
        

        var AttackerPawn = attacker.As<CCSPlayerPawn>();
        if (AttackerPawn == null || !AttackerPawn.IsValid)
            return;
        

        var AttackerController = AttackerPawn.Controller.Value?.As<CCSPlayerController>();
        if (AttackerController == null || !AttackerController.IsValid)
            return;
        

        var AttackerPlayer = _helpers.GetPlayerByController(AttackerController, _core);
        if (AttackerPlayer == null || !AttackerPlayer.IsValid)
            return;
        
        var activeWeapon = AttackerPawn.WeaponServices?.ActiveWeapon.Value;
        if (activeWeapon == null || !activeWeapon.IsValid)
            return;
        
        string customName = activeWeapon.AttributeManager.Item.CustomName;
        if (string.IsNullOrEmpty(customName))
            return;

        var dataConfig = _weaponCfg.CurrentValue;

        var weaponConfig = dataConfig.WeaponsList.FirstOrDefault(w => w.CustomName == customName);
        if (weaponConfig == null)
            return;

        if (activeWeapon != null)
        {
            if (string.IsNullOrEmpty(weaponConfig.Damage))
                return;

            var (operation, value) = _helpers.ParseDamageOperation(weaponConfig.Damage);
            if (operation != '\0')
            {
                switch (operation)
                {
                    case '+':
                        @event.Info.Damage += value;
                        break;
                    case '*':
                        @event.Info.Damage *= value;
                        break;
                    case '/':
                        if (value != 0) @event.Info.Damage /= value;
                        break;
                }
            }

            //AttackerPlayer.SendMessage(MessageType.Chat, $"¹¥»÷Ôì³ÉÉËº¦ {@event.Info.Damage} ¶îÍâÉËº¦");

        }

    }

    private HookResult OnPlayerHurt(EventPlayerHurt @event)
    {
        var victim = @event.UserIdPlayer;
        if (victim == null || !victim.IsValid)
            return HookResult.Continue;

        var victimpawn = victim.PlayerPawn;
        if (victimpawn == null || !victimpawn.IsValid)
            return HookResult.Continue;

        var attacker = _core.PlayerManager.GetPlayer(@event.Attacker);
        if (attacker == null || !attacker.IsValid)
            return HookResult.Continue;

        var attackerpawn = attacker.PlayerPawn;
        if (attackerpawn == null || !attackerpawn.IsValid)
            return HookResult.Continue;

        if(victim == attacker || victimpawn.TeamNum == attackerpawn.TeamNum)
            return HookResult.Continue;

        var activeWeapon = attackerpawn.WeaponServices?.ActiveWeapon.Value;
        if (activeWeapon == null || !activeWeapon.IsValid)
            return HookResult.Continue;

        string customName = activeWeapon.AttributeManager.Item.CustomName;
        if (string.IsNullOrEmpty(customName))
            return HookResult.Continue;

        var dataConfig = _weaponCfg.CurrentValue;

        var weaponConfig = dataConfig.WeaponsList.FirstOrDefault(w => w.CustomName == customName);
        if (weaponConfig == null)
            return HookResult.Continue;

        if (weaponConfig.knock <= 0)
            return HookResult.Continue;

        _helpers.ApplyKnockBack(attacker, victim, weaponConfig.knock);

        return HookResult.Continue;
    }

    


}