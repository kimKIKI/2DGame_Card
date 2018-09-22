using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour {

    public GameObject manager;


    public void Save()
    {
        EsSaveDataManager em = manager.GetComponent<EsSaveDataManager>();

        ES2.Save(em.Gold, "Gold");
        ES2.Save(em.Jew, "Jew");

    }
}
