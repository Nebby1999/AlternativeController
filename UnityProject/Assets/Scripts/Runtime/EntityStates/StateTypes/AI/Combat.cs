namespace EntityStates.AI
{
    public class Combat : BaseAIState
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(!baseAI.currentDriver)
            {
                outer.SetNextStateToMain();
            }
        }
    }
}