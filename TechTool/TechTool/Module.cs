using Prism.Modularity;
using Prism.Regions;
using System;

namespace TechTool
{
    public class TechToolModule : IModule
    {
        IRegionManager _regionManager;

        public TechToolModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}