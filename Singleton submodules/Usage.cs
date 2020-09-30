namespace SubmoduleStuff
{
    using System;

    public static class Testing
    {
        static void Test()
        {
            if(ExampleSubmodule.loaded)
            {
                //That static member is both unique to ExampleSubmodule, and inherited from a base class.
            }
        }
    }
}
