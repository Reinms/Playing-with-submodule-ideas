namespace SubmoduleStuff
{
    using System;


    //The ultimate base class for a submodule
    public abstract class BaseSubmodule
    {
        //Private protected on the constructor ensures that only types within this assembly are able to successfully inherit this type.
        private protected BaseSubmodule(ManualLogSource logSource)
        {
            this._logger = logSource;
        }
   
        private protected Boolean _loaded { get; private set; }


        //A submodule specific logger reduces logging boilerplate and helps debugging.
        private protected ManualLogSource _logger { get; private set; }

        //More of these can be added as needed without changes to existing submodules
        internal virtual void AddHooks() { }
        internal virtual void RemoveHooks() { }
        internal virtual void Start() { }
    }

    //A generic layer over the base class that is used to abstract out much of the singleton functionality.
    //Due to this being a singleton model, there will only ever be one instance of BaseSubmodule per non abstract type that inherits it
    //This is generic over the same set of types, so there is a seperate static "instance" of this type per type that inherits basesubmodule.
    //This produces in effect the inheritance of static members, allowing for common static functionality to be implemented here to eliminate the usual singleton boilerplate code.
    //Also provides a generalized internal interface for the static interface of a submodule.
    public abstract class BaseSubmodule<TModule> : BaseSubmodule
        where TModule : BaseSubmodule<TModule>
    {
        //Public interface
        public static Boolean loaded => instance._loaded;


        //Non-Public interface

        //Private protected on the constructor ensures that only types within this assembly are able to successfully inherit this type.
        private protected BaseSubmodule(ManualLogSource moduleLogger) : base(moduleLogger)
        {
            if(instance != null)
            {
                ReportLoadFailure(new InvalidOperationException("Cannot create two instances of a submodule"));
            }
            instance = (TModule)this;
        }
        //The place the actual instance of the singleton is stored
        internal static TModule instance { get; private set; }
        private protected static ManualLogSource logger => instance._logger;

        
        //Static helpers only visible to types inheriting

        //Replaces all the isLoaded checks
        private protected static void CheckLoaded()
        {
            if(!loaded)
            {
                var name = typeof(TModule).Name;
                throw new InvalidOperationException($"{name} is not loaded. Please use [{nameof(SubmoduleDependencyAttribute)}(typeof({name})]");
            }
        }

        //Used to report errors during load, will be caught and cleanly prevent the submodule from loading.
        private static void ReportLoadFailure() => throw new SubmoduleLoadFailureException<TModule>();
        private static void ReportLoadFailure(Exception cause) => throw new SubmoduleLoadFailureException<TModule>(cause);

        //Lots of room for logging helpers and many other things here.
    }
}
