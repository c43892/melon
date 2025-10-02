
using Framework;
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public class ESpadePlus : Equip
    {
        public override string Name { get => "SpadePlus"; }

        public override string Description { get => "Spade +" + PlusAmount; }

        public Fixed64 PlusAmount
        {
            get => (Rule as RSpadePlus).PlusAmount;
            set => (Rule as RSpadePlus).PlusAmount = value;
        }

        public override Rule Rule { get; protected set; } = new RSpadePlus() { PlusAmount = 1 };
    }
}
