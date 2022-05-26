using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDomain;

namespace GeneticsDomain
{
    public enum CreatureGender
    {
        Male,
        Female
    }

    public class UtilsGenetics : MonoBehaviour
    {
        public static UtilsGenetics instance; // make the instance visble and usable
        private static bool _alive = false;
        
        private void Awake() {
            if (_alive == false)
            {
                instance = this;
                _alive = true;
            }
        }
        public static bool areSimilar(LivingEntity entity1, LivingEntity entity2)
        {
            Chromosome one = entity1.getChromosome();
            Chromosome two = entity2.getChromosome();
            if (one.getGenesCount() != two.getGenesCount())
            {
                return false;
            }

            float averageDifference = 0;
            averageDifference += Mathf.Abs((one.getGenes()[0] + 1) - (two.getGenes()[0] + 1));// * 2;  // motor speed
            averageDifference += Mathf.Abs((one.getGenes()[1] + 1) - (two.getGenes()[1] + 1));// * 4;  // brain speed

            averageDifference += Mathf.Abs((one.getGenes()[2] + 1) - (two.getGenes()[2] + 1));// * 4;  // smell
            averageDifference += Mathf.Abs((one.getGenes()[3] + 1) - (two.getGenes()[3] + 1));// * 3;  // sight

            averageDifference += Mathf.Abs((one.getGenes()[4] + 1) - (two.getGenes()[4] + 1));// * 4;  // food preference
            averageDifference += Mathf.Abs((one.getGenes()[5] + 1) - (two.getGenes()[5] + 1));// * 1;  // behaviour


            /*for(int geneIndex = 0; geneIndex < one.getGenesCount(); geneIndex++)
            {
                averageDifference += Mathf.Abs((one.getGenes()[geneIndex] + 1) - (two.getGenes()[geneIndex] + 1));
            }*/
            averageDifference /= one.getGenesCount();
            //averageDifference /= 18;

            if (averageDifference > 0.15)
            {
                return false;
            }

            return true;
        }

    }
}