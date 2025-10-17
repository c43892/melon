using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public class CardBlock : Block, ICardAction
    {
        public Card Card { get; set; } = null;
    }
}
