using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDic2 { 

    private string key;
    private Dictionary<string, object> Dic;

	

    public TestDic2(string key, Dictionary<string, object> data)
    {
        this.key = key;
        this.Dic = data;
    }
    

    public string this[string index]
    {
        get { return Dic[index].ToString(); }
        set { Dic[index] = value; }
    }

    public string Key
    {
        set { key = value; }
        get { return key; }
    }

    public Dictionary<string, object> Data
    {

        get { return Dic; }
    }

}
