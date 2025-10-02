
using Framework;
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public class Equip
    {
        public virtual string Name { get; }

        public virtual string Description { get; }

        public virtual Rule Rule { get; protected set; }
    }
}
