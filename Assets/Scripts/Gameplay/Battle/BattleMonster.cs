using Framework;

namespace Melon.Gameplay
{
    public class BattleMonster : BattleChar, IBattleCharWithAI
    {
        public Fixed64 Attack;

        public IBattleCharAI AI { get; private set; } = null;

        public override Battle Battle
        {
            get => base.Battle;
            set
            {
                base.Battle = value;
                AI?.Init();
            }
        }

        public void Act() => AI?.Act();
    }
}
