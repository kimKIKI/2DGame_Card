using System.Collections;
using UnityEngine;

namespace BurgZergArcade.ItemSystem
{



        public interface IISObject  {

        //name
        //value - gold value
        //icon
        //burden
        //qualitylevel

            string ISName { get; set; }
            int ISValue { get; set; }
            Sprite ISIcon { get; set; }
            int ISBurden { get; set; }
            //these got  to other  item interfaces
            //equip
            //quesrItem  flag
            //durability
            //takedamage

}
}
