
using Framework;
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public class ESpadeTimes : Equip
    {
        public override string Name { get => "SpadeTimes"; }

        public override string Description { get => "Spade ¡Á" + Times; }

        public Fixed64 Times
        {
            get => (Rule as RSpadeTimes).Times;
            set => (Rule as RSpadeTimes).Times = value;
        }

        public override Rule Rule { get; protected set; } = new RSpadeTimes() { Times = 1 };
    }
}
