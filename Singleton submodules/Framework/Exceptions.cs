namespace SubmoduleStuff
{
    using System;

    
    //Always caught internally, used to provide a path for submodules to cleanly fail on load.
    internal abstract class SubmoduleLoadFailureException : Exception
    {
        internal SubmoduleLoadFailureException(String submoduleName) : base($"{submoduleName} has reported an error during initialization and will not be loaded.") { }
        internal SubmoduleLoadFailureException(String submoduleName, Exception cause) : base($"{submoduleName} has reported an error during initiaization and will not be loaded.", cause) { }
    }

    //Generic wrapper to pull the name automatically
    internal sealed class SubmoduleLoadFailureException<TModule> : SubmoduleLoadFailureException
    {
        internal SubmoduleLoadFailureException() : base(typeof(TModule).Name) { }
        internal SubmoduleLoadFailureException(Exception cause) : base(typeof(TModule).Name, cause) { }
    }
}
