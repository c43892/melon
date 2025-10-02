using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// Provides a per-frame update callback (e.g., in a game loop).
    /// </summary>
    public interface IFarmeDriven
    {
        /// <summary>
        /// Called when time has elapsed since the previous update.
        /// </summary>
        /// <param name="deltaTime">Elapsed time in seconds since the last call.</param>
        public void OnTimeElapsed(float deltaTime);
    }

    /// <summary>
    /// Base interface for a module that can be registered in a container.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Unique module name used as the registration key.
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// Interface for modules that require an initialization step.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Performs module initialization before being used.
        /// </summary>
        public void Initialize(ModuleContainer mc);
    }

    /// <summary>
    /// Interface for receiving a callback when a new module is added to the container.
    /// </summary>
    public interface IOnModuleAdded
    {
        /// <summary>
        /// Triggered after a new module has been added to the container.
        /// </summary>
        /// <param name="module">The newly added module instance.</param>
        public void OnModuleAdded(IModule module);
    }

    /// <summary>
    /// Module container that manages registration, initialization, and add-notifications.
    /// </summary>
    public class ModuleContainer : IFarmeDriven
    {
        /// <summary>
        /// Modules indexed by their unique name.
        /// </summary>
        public Dictionary<string, IModule> Modules { get; } = new Dictionary<string, IModule>();

        /// <summary>
        /// Adds a module to the container.
        /// </summary>
        /// <param name="module">The module instance to add.</param>
        /// <exception cref="Exception">Thrown if a module with the same name already exists.</exception>
        /// <remarks>
        /// Steps:
        /// 1) Ensure the module name is unique;
        /// 2) If it implements <see cref="IInitializable"/>, call <see cref="IInitializable.Initialize"/>;
        /// 3) Register the module in the dictionary;
        /// 4) Notify all modules implementing <see cref="IOnModuleAdded"/>.
        /// </remarks>
        public void AddModule(IModule module)
        {
            if (Modules.ContainsKey(module.Name))
                throw new Exception($"Module {module.Name} already exists.");

            if (module is IInitializable initializable)
                initializable.Initialize(this);

            Modules.Add(module.Name, module);

            foreach (var mod in Modules.Values)
            {
                if (mod is IOnModuleAdded onModuleAdded)
                    onModuleAdded.OnModuleAdded(module);
            }
        }

        public IModule Get(string name)
        {
            if (Modules.TryGetValue(name, out var module))
                return module;

            throw new Exception($"Module {name} not found.");
        }

        public T Get<T>() where T : IModule
        {
            foreach (var module in Modules.Values)
            {
                if (module is T typedModule)
                    return typedModule;
            }

            throw new Exception($"Module of type {typeof(T).Name} not found.");
        }

        public void OnTimeElapsed(float deltaTime)
        {
            foreach (var module in Modules.Values)
            {
                if (module is IFarmeDriven frameDriven)
                    frameDriven.OnTimeElapsed(deltaTime);
            }
        }
    }
}
