namespace AC
{

    public interface IHarvesteable
    {
        ResourceDef resourceType {get;}
        void Harvest(int amount);
    }
}
