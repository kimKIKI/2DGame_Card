using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGItemDatabase {

    static private List<RPGItem> _items;
    static private bool _isDatabaseLoaded = false;
    static private void ValidateDatabase()
    {
        if (_items == null)
            _items = new List<RPGItem>();

        if (!_isDatabaseLoaded)
            LoadDatabase();

    }

    static public void LoadDatabase()
    {
        if (_isDatabaseLoaded) return;

        _isDatabaseLoaded = true;
        LoadDatabaseForce();
        
    }
    static public void LoadDatabaseForce()
    {
        ValidateDatabase();
        RPGItem[] resources = Resources.LoadAll<RPGItem>(@"RPGItems");
        foreach (RPGItem item in resources)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);

            }
        }
    }

    static public void ClearDatabase()
        {
        _isDatabaseLoaded = false;
        _items.Clear();
        }

    static public RPGItem GetItem(int id)
    {
        ValidateDatabase();
        foreach (RPGItem item in _items)
        {
            if (item.ItemID == id)
            {
                return ScriptableObject.Instantiate(item) as RPGItem;
            }
        }
        return null;
    }
}
