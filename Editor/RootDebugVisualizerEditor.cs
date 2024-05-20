
#if UNITY_EDITOR && ODIN_INSPECTOR && ODIN_INSPECTOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif



#if UNITY_EDITOR && ODIN_INSPECTOR
[CustomEditor(typeof(RootDebugVisualizer), true)]
[CanEditMultipleObjects]
public class RootDebugVisualizerEditor : OdinEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // This will draw the default Odin Inspector GUI, including the attributes we set up
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif