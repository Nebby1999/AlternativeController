namespace AC
{
    public interface IAITargetSelector
    {
        public void Initialize(object sender);
        public BaseAI.Target GetTarget(BaseAI baseAI, AIDriver driver);
    }
}