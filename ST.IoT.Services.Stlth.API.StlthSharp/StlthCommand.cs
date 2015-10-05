using System;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public abstract class StlthCommand
    {
        public StlthCommand()
        {
            
        }

        protected abstract string createCommandBody();
    }

}