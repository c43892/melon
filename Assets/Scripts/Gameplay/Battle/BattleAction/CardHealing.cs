using Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public class CardHealing : Healing, ICardAction
    {
        public Card Card { get; set; } = null;
    }
}
