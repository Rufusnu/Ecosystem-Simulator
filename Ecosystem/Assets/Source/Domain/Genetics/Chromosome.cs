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
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Constructor [++] #### ####
        public Chromosome()
        {
            this._genes = new List<float>();
        }
        public Chromosome(Chromosome chromosome)
        {
            this._genes = new List<float>(chromosome.getGenes());
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
            return this._genes.Count;
        }
        // ---- [--] Genes [--] ----
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Behaviour [++] #### ####
        // ---- [++] Genes Inheritance [++] ---- 
        public void inheritGenes(Chromosome mother, Chromosome father)
        {
            // make a copy of the genes
            List<float> genesMale = father.getGenes();
            List<float> genesFemale = mother.getGenes();

            if (father.getGenes().Count != father.getGenes().Count)
            {
                throw new System.Exception("<Chromosome> Cannot inherit from two chromosomes of different lenght.");
            }

            inheritParentsRandomly(genesMale, genesFemale);
            mutateGenes(this._genes);
        }
        private void inheritParentsRandomly(List<float> genesFemale, List<float> genesMale)
        {
            // reset chromosome genes
            this._genes = new List<float>();

            // randomly choose genes from parents
            int parent = 0;
            for(int geneIndex = 0; geneIndex < genesMale.Count; geneIndex++)
            {
                parent = Random.Range(0,2); // select parent randomly
                if (parent == 0)
                {   // inherit female gene
                    this._genes.Add(genesFemale[geneIndex]);
                }
                else
                {   // inherit male gene
                    this._genes.Add(genesMale[geneIndex]);
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
                toMutate = Random.Range(0,10);
                if (toMutate == 0)
                {   
                    newGenes[geneIndex] = mutateGene(newGenes[geneIndex]);
                }
            }
        }
        private float mutateGene(float gene)
        {
            // selected genes mutate randomly between -0.05 and 0.05 of current value
            // TO DO : change so it mutates less and less as it gets close to the boundries
            float mutationFactor = Configs.MutationFactor();

            float mutatedGene = gene + Random.Range(-mutationFactor, mutationFactor);
            while (mutatedGene == gene || mutatedGene < -1.0f || mutatedGene > 1.0f)
            {
                mutatedGene = gene + Random.Range(-mutationFactor, mutationFactor);
            }
            return mutatedGene;
        }
        // ---- [--] Genes Inheritance [--] ---- 
        // #### #### [--] Behaviour [--] #### ####
    }
}
