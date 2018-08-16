using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BurgZergArcade.ItemSystem
{
   // [System.Serializable]
    public class ISQuality : IISQuality
    {

        [SerializeField] string _name;
        [SerializeField] Sprite _icon;

       public ISQuality()
        {
            _name = "Conmmon";
           // _icon = new Sprite();
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public Sprite ICon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
            }
        }
    }
}
