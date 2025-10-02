using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    // the player clicks "play" button to play the selected cards
    public class PlayingCards : BattleAction
    {
        public List<Card> Cards { get; set; } = null;

        public override void Apply()
        {
            // do nothing
        }
    }
}
