using System;
using TMPro;
using UnityEngine;

namespace Melon.Effect
{

    public class EEmpty : Effect
    {
        bool started = true;
        Action onStoppedCallback = null;

        public override void Play(Action onStopped)
        {
            started = true;
            onStoppedCallback = onStopped;
        }

        public override void Stop()
        {
            started = false;
            onStoppedCallback?.Invoke();
        }

        void Update()
        {
            if (started)
            {
                started = false;
                onStoppedCallback?.Invoke();
            }
        }
    }
}
