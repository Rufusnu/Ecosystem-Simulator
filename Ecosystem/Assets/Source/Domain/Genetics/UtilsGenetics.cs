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
            for(int geneIndex = 0; geneIndex < one.getGenesCount(); geneIndex++)
            {
                averageDifference += Mathf.Abs((one.getGenes()[geneIndex] + 1) - (two.getGenes()[geneIndex] + 1));
            }
            averageDifference /= one.getGenesCount();

            if (averageDifference > 0.4)
            {
                return false;
            }

            return true;
        }

    }
}