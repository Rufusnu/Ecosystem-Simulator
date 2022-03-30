using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDomain
{
    public class Chromosome
    {
        // #### [++] Attributes [++] ####
        private float[] _genes;
        private int _genesCount;
        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        private Chromosome(int newGenesCount)
        {
            if (newGenesCount < 4)
            {
                throw new System.Exception("<Chromosome> Cannot set genes count lower than four.");
            }
            this._genesCount = newGenesCount;
            this._genes = new float[this._genesCount];
        }
        // #### [--] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Genes [++] ---- 
        public float[] getGenes()
        {
            return this._genes;
        }
        private void setGenes(float[] newGenes)
        {
            if (newGenes == null)
            {
                throw new System.Exception("<Chromosome> Cannot set null genes.");
            }
            this._genes = newGenes;
        }
        // ---- [--] Genes [--] ---- 
        // #### [--] Getters & Setters [--] ####


        // #### [++] Behaviour [++] ####
        // ---- [++] Genes Inheritance [++] ---- 
        public void inheritGenes(Chromosome parentMaleChromosome, Chromosome parentFemaleChromosome)
        {
            // make a copy of the genes
            float[] genesMale = parentMaleChromosome.getGenes();
            float[] genesFemale = parentFemaleChromosome.getGenes();

            if (parentMaleChromosome.getGenes().Length != parentMaleChromosome.getGenes().Length)
            {
                throw new System.Exception("<Chromosome> Cannot inherit from two chromosomes of different lenght.");
            }
            if (parentMaleChromosome.getGenes().Length != this._genesCount)
            {
                throw new System.Exception("<Chromosome> Cannot inherit from chromosomes of different lenght from this chromosome's lenght.");
            }

            // create empty genes
            float[] genesOffspring = new float[this._genesCount];
            inheritParentsRandomly(genesOffspring, genesMale, genesFemale);
            mutateGenes(genesOffspring);
            setGenes(genesOffspring);
        }
        private void inheritParentsRandomly(float[] genesOffspring, float[] genesMale, float[] genesFemale)
        {
            // randomly choose genes from parents
            int parent = 0;
            for(int geneIndex = 0; geneIndex < genesMale.Length; geneIndex++)
            {
                parent = Random.Range(0,1); // select parent randomly
                if (UtilsEntity.instance.intToGender(parent) == "female") // dictionary used to make code easier to follow
                {   // inherit female gene
                    genesOffspring[geneIndex] = genesFemale[geneIndex];
                }
                else
                {   // inherit male gene
                    genesOffspring[geneIndex] = genesMale[geneIndex];
                }
            }
        }
        private void mutateGenes(float[] newGenes)
        {
            // randomly mutate some of the genes
            int toMutate = 0;
            for(int geneIndex = 0; geneIndex < newGenes.Length; geneIndex++)
            {
                // randomly choose which genes to mutate
                toMutate = Random.Range(0,1);
                if (toMutate == 1)
                {   // selected genes mutate randomly between 95% and 105% of current value
                    newGenes[geneIndex] *= Random.Range(1 - EntityConfig.instance.MutationFactor, 1 + EntityConfig.instance.MutationFactor);
                }
            }
        }
        // ---- [--] Genes Inheritance [--] ---- 
        // #### [--] Behaviour [--] ####
    }
}
