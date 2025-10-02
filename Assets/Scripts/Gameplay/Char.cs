
namespace Melon.Gameplay
{
    public enum CharType
    {
        None = 0,

        Hero =      0x1000, // the last bit represents corresponding card suit
        Warrior =   Hero | 0x0010 | CardSuit.Clubs,
        Priest =    Hero | 0x0020 | CardSuit.Hearts,
        Mage =      Hero | 0x0030 | CardSuit.Spades,
        Guard =     Hero | 0x0040 | CardSuit.Diamonds,

        // Monsters
        Monster =   0x2000,
        SmallSlime  = Monster | 0x0001,
        BigSlime    = Monster | 0x0002
    }

    public class Char
    {
        public CharType Type = CharType.None;
    }
}
