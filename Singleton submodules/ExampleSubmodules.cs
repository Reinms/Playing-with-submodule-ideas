namespace SubmoduleStuff
{
    using System;

    [Submodule(true)]
    public sealed class ExampleSubmodule : BaseSubmodule<ExampleSubmodule>
    {
        //The only line of boilerplate code required in a submodule
        internal ExampleSubmodule(ManualLogSource logger) : base(logger) { }

        //Internal functionality stuff
        internal override void AddHooks() => base.AddHooks();
        internal override void RemoveHooks() => base.RemoveHooks();
        internal override void Start() => base.Start();



        //Public interface
        [RequiresLoaded]
        public static void SomeAPIFeature()
        {
            CheckLoaded();
            //Do some API stuff
        }

        [NoLoadRequirement]
        public static void SomeHelperMethod()
        {
            //Do some stuff that doesn't require the api to be loaded
        }
    }
}
