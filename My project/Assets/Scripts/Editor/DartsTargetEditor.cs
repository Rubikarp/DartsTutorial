using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DartTarget))]
public class DartsTargetEditor : Editor
{
    float DoubleOuterRadius;

    public void OnSceneGUI()
    {
        DartTarget obj = (target as DartTarget);

        EditorGUI.BeginChangeCheck();
        Matrix4x4 matrix = Matrix4x4.TRS(obj.transform.position, obj.transform.rotation, Vector3.one);
        using (new Handles.DrawingScope(Color.green, matrix))
        {
            DoubleOuterRadius = Handles.RadiusHandle(Quaternion.identity, Vector3.zero, obj.DoubleOuterRadius, false);
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
            obj.DoubleOuterRadius = DoubleOuterRadius;
        }
    }
}
