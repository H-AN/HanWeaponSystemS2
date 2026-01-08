using System.Numerics;
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
    public void GiveWeaponAndSkin(IPlayer player, HanWeaponSystemConfig.Weapons datas)//string classname, string vdataname, string customname, int slot, int definition, int maxclip, int reserveammo, float Rate, string path
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
                _logger.LogError("slot 错误!!");
                return;
        }

        var Is = pawn.ItemServices;
        if (Is == null || !Is.IsValid)
            return;

        var weapon = Is.GiveItem<CCSWeaponBase>(classname);
        if (weapon == null || !weapon.IsValid)
        {
            _logger.LogError("创建失败: {ClassName}", classname);
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

        var KnockBack = CalculateKnockbackDirection(attakcer, target, force);

        var pawn = target.PlayerPawn;
        if (pawn == null || !pawn.IsValid)
            return;

        pawn.AbsVelocity = KnockBack;
    }
    public SwiftlyS2.Shared.Natives.Vector CalculateKnockbackDirection(IPlayer attacker, IPlayer target, float force)
    {
        if (!attacker.IsValid)
            return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        var attackerpawn = attacker.PlayerPawn;
        if (attackerpawn == null || !attackerpawn.IsValid)
            return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        var attackerPos = attackerpawn.AbsOrigin;
        if (attackerPos == null)
            return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        var targetpawn = target.PlayerPawn;
        if (targetpawn == null || !targetpawn.IsValid)
            return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        var targetPos = targetpawn.AbsOrigin;
        if (targetPos == null)
            return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        var dir = new SwiftlyS2.Shared.Natives.Vector(
            targetPos.Value.X - attackerPos.Value.X,
            targetPos.Value.Y - attackerPos.Value.Y,
            targetPos.Value.Z - attackerPos.Value.Z
        );

        float length = MathF.Sqrt(dir.X * dir.X + dir.Y * dir.Y + dir.Z * dir.Z);
        if (length <= 0.01f) return new SwiftlyS2.Shared.Natives.Vector(0, 0, 0);

        return new SwiftlyS2.Shared.Natives.Vector(
            dir.X / length * force,
            dir.Y / length * force,
            50f
        );
    }



    public IPlayer? GetPlayerBySteamID(ulong? SteamID, ISwiftlyCore core)
    {
        return core.PlayerManager.GetAllPlayers().FirstOrDefault(x => !x.IsFakeClient && x.SteamID == SteamID);
    }

    public IPlayer? GetPlayerByController(CCSPlayerController controller, ISwiftlyCore core)
    {
        return core.PlayerManager.GetPlayer((int)(controller.Index - 1));
    }

}
