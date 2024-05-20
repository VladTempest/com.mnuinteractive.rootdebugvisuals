using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#endif

public enum DefaultColliderType
{
    Cube,
    Sphere
}

[ExecuteInEditMode]
public class RootDebugVisualizer : MonoBehaviour
{
#if UNITY_EDITOR

#if ODIN_INSPECTOR
    [Title("Collider Settings")]
    [ValidateInput("HasValidTargetCollider", "If target collider is null, default collider type will be used.")]
#endif
    public Collider targetCollider;

#if ODIN_INSPECTOR
    [HideIf("IsTargetColliderNull")]
    [ToggleLeft]
#endif
    public bool isSizeRewritten;

#if ODIN_INSPECTOR
    [ShowIf("IsTargetColliderNull")]
    [EnumToggleButtons]
    [BoxGroup("Default Collider Settings")]
    [LabelText("Default Collider Type")]
#endif
    public DefaultColliderType defaultColliderType = DefaultColliderType.Cube;

#if ODIN_INSPECTOR
    [BoxGroup("Transform Parameters")]
    [ShowIf("ShouldShowCubeSize")]
    [LabelText("Cube Size")]
#endif
    public Vector3 cubeSize = Vector3.one;

#if ODIN_INSPECTOR
    [BoxGroup("Transform Parameters")]
    [ShowIf("ShouldShowSphereRadius")]
    [LabelText("Sphere Radius")]
#endif
    public float sphereRadius = 0.5f;

#if ODIN_INSPECTOR
    [BoxGroup("Transform Parameters")]
    [LabelText("Center Offset")]
#endif
    public Vector3 centerOffset = Vector3.zero;

#if ODIN_INSPECTOR
    [BoxGroup("Debug Settings")]
    [ColorPalette]
#endif
    public Color debugColor = Color.green;

    private bool IsTargetColliderNull()
    {
        return targetCollider == null;
    }
    
    private bool ShouldShowCubeSize()
    {
        return ( (isSizeRewritten && targetCollider != null && targetCollider is BoxCollider)) || 
               (targetCollider == null && defaultColliderType == DefaultColliderType.Cube);
    }

    private bool ShouldShowSphereRadius()
    {
        return ((isSizeRewritten && targetCollider != null && targetCollider is SphereCollider)) || 
               (targetCollider == null && defaultColliderType == DefaultColliderType.Sphere);
    }

    private bool HasValidTargetCollider()
    {
        return targetCollider != null || GetComponent<Collider>() != null;
    }

    void OnDrawGizmos()
    {
        Vector3 scale = transform.localScale;

        if (targetCollider == null)
        {
            if (defaultColliderType == DefaultColliderType.Cube)
            {
                DrawCube(transform.position + centerOffset, transform.rotation, Vector3.Scale(cubeSize, scale));
            }
            else if (defaultColliderType == DefaultColliderType.Sphere)
            {
                DrawSphere(transform.position + centerOffset, transform.rotation, sphereRadius * Mathf.Max(scale.x, scale.y, scale.z));
            }
            return;
        }

        if (targetCollider is BoxCollider boxCollider)
        {
            Vector3 size = isSizeRewritten ? cubeSize : boxCollider.size;
            DrawCube(boxCollider.transform.position + centerOffset, boxCollider.transform.rotation, Vector3.Scale(size, scale));
        }
        else if (targetCollider is SphereCollider sphereCollider)
        {
            float radius = isSizeRewritten ? sphereRadius : sphereCollider.radius;
            DrawSphere(sphereCollider.transform.position + centerOffset, sphereCollider.transform.rotation, radius * Mathf.Max(scale.x, scale.y, scale.z));
        }
    }

    void DrawCube(Vector3 position, Quaternion rotation, Vector3 size)
    {
        Gizmos.color = debugColor;
        Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }

    void DrawSphere(Vector3 position, Quaternion rotation, float radius)
    {
        Gizmos.color = debugColor;
        Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
    }
    
#endif
}