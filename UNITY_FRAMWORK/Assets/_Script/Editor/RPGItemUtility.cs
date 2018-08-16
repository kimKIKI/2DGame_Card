using System.Collections;

using UnityEditor;
using UnityEngine;

public class RPGItemUtility  {

    [MenuItem("Assets/Create/RPG/Item")]
    static public void CreateItem()
    {
        ScriptableObjectUtility.CreateAsset<RPGItem>();
    }
}
