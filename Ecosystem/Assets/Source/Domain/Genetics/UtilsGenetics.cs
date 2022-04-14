using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticsDomain
{
    public class UtilsGenetics : MonoBehaviour
    {
        public static UtilsGenetics instance; // make the instance visble and usable
        

        private void Awake() {
            instance = this;    
            constructDictionary();
        }

        // #### [++] Integer To Gender [++] ####
        private Dictionary<int, string> gender; // used in <Chromosome> to make code easier to follow
        private void constructDictionary()
        {   // the dictionary translates int values 0,1 to human understandable values
            this.gender.Add(0, "female");
            this.gender.Add(1, "male");
        }
        public string intToGender(int integer) // used in <Chromosome> to make code easier to follow
        {
            return this.gender[integer];
        }
        // #### [--] Integer To Gender [--] ####

    }
}