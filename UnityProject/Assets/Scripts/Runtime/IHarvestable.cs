namespace AC
{
    /// <summary>
    /// Representa que un objeto se puede cosechar.
    /// </summary>
    public interface IHarvestable
    {
        /// <summary>
        /// El tipo de recurso en este deposito
        /// </summary>
        ResourceDef resourceType { get; }

        /// <summary>
        /// Metodo llamado cuando algo cosecha de este objeto.
        /// </summary>
        /// <param name="amount">La cantidad cosechada.</param>
        void Harvest(int amount);
    }
}