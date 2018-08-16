using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BurgZergArcade.ItemSystem
{

    public class ISObject : IISObject
    {
        [SerializeField]
        Sprite _icon;
        [SerializeField]
        string _name;
        [SerializeField]
        int _value;
        [SerializeField]
        int _burden;
        [SerializeField]
        ISQuality _quality;


        public string ISName
        {
            get { return _name; }

            set { _name = value; }

        }

        public int ISValue
        {
            get { return _value; }

            set { _value = value; }

        }

        public Sprite ISIcon
        {
            get { return _icon; }

            set { _icon = value; }

        }

        public int ISBurden
        {
            get { return _burden; }

            set { _burden = value; }

        }
    }
}
