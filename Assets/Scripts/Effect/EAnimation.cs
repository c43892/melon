using System;
using TMPro;
using UnityEngine;

namespace Melon.Effect
{

    public class EAnimation : Effect
    {
        [SerializeField]
        public Animation Animation;

        bool started = false;
        Action onStoppedCallback = null;

        public override void Play(Action onStopped)
        {
            started = true;
            onStoppedCallback = onStopped;
            Animation.Play();
        }

        public override void Stop()
        {
            Animation.Stop();
            started = false;
            onStoppedCallback?.Invoke();
        }

        void Update()
        {
            if (!Animation.isPlaying && started)
            {
                started = false;
                onStoppedCallback?.Invoke();
            }
        }
    }
}
