using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BurgZergArcade.ItemSystem
{

    public class ISQualityDatabase : ScriptableObject
    {
          [SerializeField]
      public  List<ISQuality> database = new List<ISQuality>();


        public void Add(ISQuality item)
        {
            database.Add(item);
        }
    }
}
