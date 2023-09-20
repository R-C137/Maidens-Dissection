using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NovelScript))]
public class SelectAll : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        string searchName = GUILayout.TextField("Speaker name");

        if(GUILayout.Button("Conditional Select"))
        {
            var objs = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/Novel Script/Act 1/Part 1").Where((obj) => ((NovelScript)obj).speaker == searchName);

            Selection.objects = objs.ToArray();
        }
    }
}
