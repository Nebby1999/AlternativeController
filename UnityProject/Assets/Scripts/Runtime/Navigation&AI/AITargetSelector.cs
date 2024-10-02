namespace AC
{
    public interface IAITargetSelector
    {
        public BaseAI.Target GetTarget(BaseAI baseAI, AIDriver driver);
    }
}