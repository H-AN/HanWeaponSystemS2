using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Cecil.Cil;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Helpers;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using static HanWeaponSystemS2.HanWeaponSystemHelpers;


namespace HanWeaponSystemS2;

public class HanWeaponSystemHelpers
{
    private readonly ILogger<HanWeaponSystemHelpers> _logger;
    private readonly ISwiftlyCore _core;

    public HanWeaponSystemHelpers(ISwiftlyCore core, ILogger<HanWeaponSystemHelpers> logger)
    {
        _core = core;
        _logger = logger;
    }

    public class WeaponData
    {
        public int ClipSize { get; set; }
        public int ReserveAmmo { get; set; }
    }
    public void GiveWeaponAndSkin(IPlayer player, HanWeaponSystemConfig.Weapons datas)
    { 
        var pawn = player.PlayerPawn;
        if (pawn == null || !pawn.IsValid)
            return;

        var classname = datas.ClassName;
        var vdataname = datas.VdataName;
        var customname = datas.CustomName;
        var slot = datas.Slot;
        var definition = datas.Definition;
        var maxclip = datas.MaxClip;
        var reserveammo = datas.ReserveAmmo;
        var rate = datas.Rate;
        var modelpath = datas.PrecacheModel;

        var ws = pawn.WeaponServices;
        if (ws == null || !ws.IsValid)
            return;

        switch (slot)
        {
            case 0:
                ws.DropWeaponBySlot(gear_slot_t.GEAR_SLOT_RIFLE);
                break;
            case 1:
                ws.DropWeaponBySlot(gear_slot_t.GEAR_SLOT_PISTOL);
                break;
            case 2:
                ws.DropWeaponBySlot(gear_slot_t.GEAR_SLOT_KNIFE);
                break;
            case 3:
                ws.DropWeaponBySlot(gear_slot_t.GEAR_SLOT_GRENADES);
                break;
            default:
                _logger.LogError("slot Error!!");
                return;
        }

        var Is = pawn.ItemServices;
        if (Is == null || !Is.IsValid)
            return;

        var weapon = Is.GiveItem<CCSWeaponBase>(classname);
        if (weapon == null || !weapon.IsValid)
        {
            _logger.LogError("Create Error: {ClassName}", classname);
            return;
        }
        weapon.AcceptInput("ChangeSubclass", vdataname);

        weapon.AttributeManager.Item.CustomName = customname;
        weapon.AttributeManager.Item.CustomNameOverride = customname;
        if (weapon.AttributeManager.Item.CustomName == customname)
        {

            weapon.AttributeManager.Item.ItemDefinitionIndex = (ushort)definition;
            
            var WeaponBase = weapon.WeaponBaseVData;
            if (WeaponBase.IsValid)
            {
                WeaponBase.PrimaryReserveAmmoMax = reserveammo;
                WeaponBase.SecondaryReserveAmmoMax = reserveammo;
                WeaponBase.CycleTime.Values[0] = rate;
                WeaponBase.CycleTime.Values[1] = rate;
                WeaponBase.DefaultClip1 = maxclip;
                WeaponBase.MaxClip1 = maxclip;

                WeaponBase.DefaultClip2 = reserveammo;
                WeaponBase.MaxClip2 = reserveammo;
            }


            var PlayerWeaponBase = weapon.PlayerWeaponVData;
            if (PlayerWeaponBase.IsValid)
            {
                PlayerWeaponBase.MaxClip1 = maxclip;
                weapon.Clip1 = maxclip;
                weapon.Clip1Updated();

                PlayerWeaponBase.MaxClip2 = reserveammo;
                weapon.Clip2 = reserveammo;
                weapon.Clip2Updated();
            }
            

        }
    }


    public (char operation, float value) ParseDamageOperation(string damageConfig)
    {
        if (string.IsNullOrEmpty(damageConfig))
            return ('\0', 0f);

        damageConfig = damageConfig.Trim();

        char operation;
        string valueStr;

        if (damageConfig[0] == '+' || damageConfig[0] == '*' || damageConfig[0] == '/')
        {
            operation = damageConfig[0];
            valueStr = damageConfig.Substring(1);
        }
        else
        {
            operation = '+';
            valueStr = damageConfig.TrimStart('-', '+');
        }

        if (float.TryParse(valueStr, out float value))
        {
            return (operation, value);
        }

        return ('\0', 0f);
    }

    public (char operation, float value) ParseRateOperation(string rateConfig)
    {
        if (string.IsNullOrWhiteSpace(rateConfig))
            return ('\0', 0f);

        rateConfig = rateConfig.Trim();

        char operation;
        string valueStr;

        if (rateConfig[0] == '+' || rateConfig[0] == '-' || rateConfig[0] == '*' || rateConfig[0] == '/')
        {
            operation = rateConfig[0];
            valueStr = rateConfig.Substring(1);
        }
        else
        {
            // 无符号 → 默认 +
            operation = '+';
            valueStr = rateConfig;
        }

        if (float.TryParse(valueStr, out float value))
        {
            return (operation, value);
        }

        return ('\0', 0f);
    }

    public void ApplyKnockBack(IPlayer attakcer, IPlayer target, float force)
    {
        if (attakcer == null || !attakcer.IsValid)
            return;

        if (target == null || !target.IsValid)
            return;

        if (force <= 0)
            return;

        var attakcerpawn = attakcer.PlayerPawn;
        if (attakcerpawn == null || !attakcerpawn.IsValid)
            return;

        var Rotation = attakcerpawn.AbsRotation;
        if (Rotation == null)
            return;

        QAngle Angle = Rotation.Value;
        Angle.ToDirectionVectors(out Vector vecKnockback, out _, out _);
        var pushVelocity = vecKnockback * force;

        var targetPawn = target.PlayerPawn;
        if (targetPawn == null || !targetPawn.IsValid)
            return;

        var vel = targetPawn.AbsVelocity;
        targetPawn.Teleport(null, null, vel + pushVelocity);
    }
    
}


