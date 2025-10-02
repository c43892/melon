using Melon.Gameplay;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

namespace Melon.UI
{
    public class UIEquip : MonoBehaviour
    {
        [SerializeField]
        Text Description;

        public Equip Equip { get; set; }

        public void Refresh()
        {
            if (Equip != null)
                Description.text = Equip.Description;
            else
                Description.text = "";
        }
    }
}
