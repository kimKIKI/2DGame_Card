using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

namespace JSONFactory
{
    class JSONAssembly {
        private static Dictionary<int, string> resourceList = new Dictionary<int, string>()
        {  // add your json file here
            {1, "Data/test.json" }   
        };


        //public static JSONStructure RunJSONFactory(int indexNumber)
        //{
        //    string resourcePath = FindPath(indexNumber);
        //    if (Path.GetExtension(resourcePath) == ".json")                                   // Is valid ??
        //    {
        //        string jsonString = File.ReadAllText(Application.dataPath + "/" + resourcePath);  // get all string from the file
        //        JSONStructure jsonData = JsonMapper.ToObject<JSONStructure>(jsonString);   //convert the json string into the structure
        //        return jsonData;
        //    }
        //    else
        //    {
        //        throw new Exception("the file is not json data");
        //    }
        //}

        private static string FindPath(int indexNumber)
        {
            string resourcePath;
            if (resourceList.TryGetValue(indexNumber, out resourcePath) == true) // get value from the dictionary using key
            {
                return resourcePath;
            }
            else
            {
                //using System;
                throw new Exception("The index number you provided is not in the resource List");
            }

        }
     }
}
