namespace SubmoduleStuff
{
    using System;

    //Nothing new here really. Just switched to using types for dependencies since they are easier to check for validity.
    //Also with a more defined failure process its a bit easier 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SubmoduleDependencyAttribute : Attribute
    {
        internal SubmoduleDependencyAttribute(Boolean unloadMeIfMissing, params Type[] dependencies)
        {
            this.unloadMeIfMissing = unloadMeIfMissing;
            this.dependencies = dependencies;
        }

        internal Boolean unloadMeIfMissing;
        internal Type[] dependencies;
    }



    //Used to flag something as a submodule. Not strictly needed, but is maybe good to have as a way for submodules to provide information.
    //As an example case, this would have a bool to maybe toggle the submodule on and off.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class SubmoduleAttribute : Attribute
    {
        internal SubmoduleAttribute(Boolean functional, params Type[] dependencies)
        {
            this.functional = functional;
            this.dependencies = dependencies;
        }

        internal Boolean functional;
        internal Type[] dependencies;
    }





    //Base class for three primarily annotation based attributes used to mark when a method is intended to be called
    //Could be integrated with tests down the line?
    internal abstract class LoadRequirementsAttribute : Attribute
    {
        private protected enum CallLoadRequirement
        {
            DuringLoad,
            AfterLoad,
            NoLoadRequirement,
        }
        private protected abstract CallLoadRequirement loadRequirement { get; }
    }

    /// <summary>
    /// Indicates that a method is called during the load of the submodule.
    /// Should never contain a call to CheckLoaded().
    /// All exceptions and error cases should be handled and reported through ReportLoadFailure() or ReportLoadFailure(Exception)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class CalledDuringLoadAttribute : LoadRequirementsAttribute
    {
        private protected sealed override CallLoadRequirement loadRequirement => CallLoadRequirement.DuringLoad;
    }

    /// <summary>
    /// This method requires the submodle to be loaded when called.
    /// Should always begin with a call to CheckLoaded()
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class RequiresLoadedAttribute : LoadRequirementsAttribute
    {
        private protected sealed override CallLoadRequirement loadRequirement => CallLoadRequirement.AfterLoad;
    }

    /// <summary>
    /// This method does not require the submodule to be loaded when called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class NoLoadRequirementAttribute : LoadRequirementsAttribute
    {
        private protected sealed override CallLoadRequirement loadRequirement => CallLoadRequirement.NoLoadRequirement;
    }
}
