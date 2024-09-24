using System;

namespace AC
{

    [Obsolete]
    public interface IHarvesteable_OLD
    {
        ResourceDef resourceType {get;}
        void Harvest(int amount);
    }
}
