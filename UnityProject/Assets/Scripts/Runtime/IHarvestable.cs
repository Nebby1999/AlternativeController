namespace AC
{
    public interface IHarvestable
    {
        ResourceDef resourceType { get; }
        void Harvest(int amount);
    }
}