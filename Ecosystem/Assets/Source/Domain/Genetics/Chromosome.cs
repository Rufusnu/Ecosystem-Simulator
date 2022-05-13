using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDomain;

namespace GeneticsDomain
{
    public class Chromosome
    {
        // #### #### [++] Attributes [++] #### ####
        private List<float> _genes = new List<float>();
        private int _genesCount;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Constructor [++] #### ####
        public Chromosome()
        {
            this._genesCount = 0;
        }
        // #### #### [--] Constructor [--] #### ####


        // #### #### [++] Getters & Setters [++] #### ####
        // ---- [++] Genes [++] ---- 
        public List<float> getGenes()
        {
            return this._genes;
        }
        private void setGenes(List<float> newGenes)
        {
            if (newGenes == null)
            {
                throw new System.Exception("<Chromosome> Cannot set null genes.");
            }
            this._genes = newGenes;
        }
        public void addGene(float gene)
        {
            this._genes.Add(gene);
        }
        public int getGenesCount()
        {
            return this._genesCount;
        }
        // ---- [--] Genes [--] ----
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Behaviour [++] #### ####
        // ---- [++] Genes Inheritance [++] ---- 
        public void inheritGenes(Chromosome parentMaleChromosome, Chromosome parentFemaleChromosome)
        {
            // make a copy of the genes
            List<float> genesMale = parentMaleChromosome.getGenes();
            List<float> genesFemale = parentFemaleChromosome.getGenes();

            if (parentMaleChromosome.getGenes().Count != parentMaleChromosome.getGenes().Count)
            {
                throw new System.Exception("<Chromosome> Cannot inherit from two chromosomes of different lenght.");
            }

            // create empty genes
            inheritParentsRandomly(this._genes, genesMale, genesFemale);
            mutateGenes(this._genes);
        }
        private void inheritParentsRandomly(List<float> genesOffspring, List<float> genesMale, List<float> genesFemale)
        {
            // randomly choose genes from parents
            int parent = 0;
            for(int geneIndex = 0; geneIndex < genesMale.Count; geneIndex++)
            {
                parent = Random.Range(0,1); // select parent randomly
                if (UtilsGenetics.instance.intToGender(parent) == "female") // dictionary used to make code easier to follow
                {   // inherit female gene
                    genesOffspring.Add(genesFemale[geneIndex]);
                }
                else
                {   // inherit male gene
                    genesOffspring.Add(genesMale[geneIndex]);
                }
            }
        }
        private void mutateGenes(List<float> newGenes)
        {
            // randomly mutate some of the genes
            int toMutate = 0;
            for(int geneIndex = 0; geneIndex < newGenes.Count; geneIndex++)
            {
                // randomly choose which genes to mutate
                toMutate = Random.Range(0,1);
                if (toMutate == 1)
                {   
                    newGenes[geneIndex] = mutateGene(newGenes[geneIndex]);
                }
            }
        }
        private float mutateGene(float gene)
        {
            // selected genes mutate randomly between -0.05 and 0.05 of current value
            // TO DO : change so it mutates less and less as it gets close to the boundries

            float mutatedGene = gene + Random.Range(-EntityConfig.instance.MutationFactor, EntityConfig.instance.MutationFactor);
            while (mutatedGene == gene || mutatedGene < -1.0f || mutatedGene > 1.0f)
            {
                mutatedGene = gene + Random.Range(-EntityConfig.instance.MutationFactor, EntityConfig.instance.MutationFactor);
            }
            return mutatedGene;
        }
        // ---- [--] Genes Inheritance [--] ---- 
        // #### #### [--] Behaviour [--] #### ####
    }
}
