using Nebula.Editor.Inspectors;

namespace AC.Editor
{
    [UnityEditor.CustomEditor(typeof(AC.EntityStateMachine))]
    public class EntityStateMachineInspector : StateMachineInspector<EntityStateMachine>
    {

    }
}