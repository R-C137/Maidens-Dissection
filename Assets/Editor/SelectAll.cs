using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SelectAll : MonoBehaviour
{
    public static void SelectAllByName(string search)
    {
       List<NovelScript> scripts = new List<NovelScript>();

       scripts.Add(AssetDatabase.LoadAssetAtPath<NovelScript>("Assets/Novel Script/Act 1/Part 1/NovelScript.asset"));

       for (int i = 1; i < 215; i++) 
       { 
           scripts.Add(AssetDatabase.LoadAssetAtPath<NovelScript>($"Assets/Novel Script/Act 1/Part 1/NovelScript {i}.asset"));
       }

       List<NovelScript> sortedScripts = new();
       
       foreach(NovelScript script in scripts)
       {
           try
           {
               if (script.speaker == search)
                   sortedScripts.Add(script);
           }catch(Exception)
           {
           }
       }
       Selection.objects = sortedScripts.ToArray();
        
    }

    [MenuItem("Tools/SelectAll/Has Choice")]
    public static void SelectAllChoices()
    {
        List<NovelScript> scripts = new List<NovelScript>();

        scripts.Add(AssetDatabase.LoadAssetAtPath<NovelScript>("Assets/Novel Script/Acts/Act 1/Part 1/NovelScript.asset"));

        for (int i = 1; i < 215; i++)
        {
            scripts.Add(AssetDatabase.LoadAssetAtPath<NovelScript>($"Assets/Novel Script/Acts/Act 1/Part 1/NovelScript {i}.asset"));
        }

        List<NovelScript> sortedScripts = new();

        foreach (NovelScript script in scripts)
        {
            try
            {
                if (script.choices.Any() )
                    sortedScripts.Add(script);
            }
            catch (Exception)
            {
            }
        }
        Selection.objects = sortedScripts.ToArray();
    }

    [MenuItem("Tools/SelectAll/MC")]
    public static void SelectAllMC()
    {
        SelectAllByName("MC");
    }

    [MenuItem("Tools/SelectAll/Ivy")]
    public static void SelectAllIvy()
    {
        SelectAllByName("Ivy");
    }

    [MenuItem("Tools/SelectAll/Camille")]
    public static void SelectAllCamille()
    {
        SelectAllByName("Camille");
    }

    [MenuItem("Tools/SelectAll/Aster")]
    public static void SelectAllAster()
    {
        SelectAllByName("Aster");
    }
}
