using UnityEngine;

namespace Framework
{
    public class GameCore : ModuleContainer
    {
        public void Initialize()
        {
            foreach (var module in Modules.Values)
            {
                if (module is IInitializable initializable)
                    initializable.Initialize(this);
            }

            // Initialization logic here
            Debug.Log("GameCore initialized.");
        }
    }
}
