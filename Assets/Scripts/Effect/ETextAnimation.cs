using System;
using TMPro;
using UnityEngine;

namespace Melon.Effect
{
    public class ETextAnimation : EAnimation, ITextEffect
    {
        [SerializeField]
        TextMeshPro TextMesh;

        public string Text
        {
            set => TextMesh.text = value;
        }
    }
}
