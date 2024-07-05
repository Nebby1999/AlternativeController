public interface IHarvesteable
{
    MineralType Type {get;}
    void Harvest(int amount);
}
