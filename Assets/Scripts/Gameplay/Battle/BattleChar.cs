using Framework;

namespace Melon.Gameplay
{
    public class BattleChar
    {
        public Char Char { get; set; } = null;

        public Fixed64 MaxHp { get; set; } = 0;

        public Fixed64 Hp { get; set; } = 0;

        public Fixed64 HpHealingTop { get; set; } = 0;

        public Fixed64 Block { get; set; } = 0;
    }
}
