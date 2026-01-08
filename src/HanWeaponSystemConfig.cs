
namespace HanWeaponSystemS2;

public class HanWeaponSystemConfig
{
    public class Weapons
    {
        public string CustomName { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string VdataName { get; set; } = string.Empty;
        public int Definition { get; set; } = 0;
        public string Damage { get; set; } = string.Empty;
        public float knock { get; set; } = 0;
        public int MaxClip { get; set; } = 0;
        public int ReserveAmmo { get; set; } = 0;
        public float Rate { get; set; } = 0;
        public bool NoRecoil { get; set; } = false;
        public string KillIcon { get; set; } = string.Empty;
        public int Slot { get; set; } = 0;
        public string PrecacheModel { get; set; } = string.Empty;
        public string PrecacheSoundEvent { get; set; } = string.Empty;


    }
    public List<Weapons> WeaponsList { get; set; } = new List<Weapons>();

}



