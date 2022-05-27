 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;
using Unity.Mathematics;
using EntityDomain;
using GeneticsDomain;


namespace SmellDomain
{
    public class SmellNode : Entity
    {
        // A creature will find any of the nodes and follow them towards [Node0]

        //  |    node - intensity    |    node - intensity    |    node - intensity   |
        //  |  [Node0] [    3    ] <--> [Node1] [    2    ] <--> [Node3] [    1    ]  |

        // Node 0 is always the GOAL node


        protected LivingEntity _whose;
        protected Color _color;
        protected Color _lineColor;
        protected Entity _nextNode;
        protected SmellNode _previousNode;
        protected int _intensity;

        public SmellNode(LivingEntity entity) : base(entity.getCoordinates())
        {
            // used to start a new chain of Nodes
            this._whose = entity;
            int intensity = this._whose.getSmellIntensity();   // obtain intensity from creature genes
            this._nextNode = this._whose;
            
            if (this.setIntensity(intensity) == false)
            {
                throw new System.Exception("<SmellNode> Cannot construct a smell intensity <= 0!");
            }
            this._previousNode = null;
            initializeObject();
            GridMap.currentGridInstance.addSmell(this);
        }

        public SmellNode(LivingEntity entity, SmellNode currentNode) : base(entity.getCoordinates())
        {
            // used to continue a chain of existing nodes
            this._whose = entity;
            int intensity = this._whose.getSmellIntensity();   // obtain intensity from creature genes
            this._nextNode = this._whose;

            if (this.setIntensity(intensity) == false)
            {
                throw new System.Exception("<SmellNode> Cannot construct a smell intensity <= 0!");
            }
            this.setPreviousNode(currentNode);
            initializeObject();
            GridMap.currentGridInstance.addSmell(this);

            if (this != null && this._previousNode != null)
                if (this.getObject() != null && this._previousNode.getObject() != null)
                {
                    Debug.DrawLine(this.getObject().transform.position, this._previousNode.getObject().transform.position, this._lineColor, 12.0f);
                }
        }

        private void initializeObject()
        {
            this.setObject(new GameObject("<Smell of " + this._whose.getObject().name + ">"));
            // settings position
            this.getObject().transform.SetParent(this._whose.getObject().transform.parent);
            Vector3 whosePosition = this._whose.getObject().transform.localPosition;
            this.getObject().transform.localPosition = new Vector3(whosePosition.x, whosePosition.y, -3);

            // setting sprite
            // Sprite color is gene dependant
            this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.smell_default;
            updateSpriteColor();
        }

        private void updateSpriteColor()
        {
            if (this.getObject() == null)
            {
                return;
            }
            this._color = this._whose.getObject().GetComponent<SpriteRenderer>().color;
            this._color = new Color(this._color.r, this._color.g, this._color.b, Configs.SmellSpriteAlpha());
            this._lineColor = new Color(this._color.r, this._color.g, this._color.b, 0.9f);
            this.getObject().GetComponent<SpriteRenderer>().color = this._color;
        }

        public Entity getNext()
        {
            // returns next node if exists, else it returns the entity whose smell it is
            // this way, it can obtain its coordinates because both classes are inherited
            // from <Entity> class that has coordinates

            return this._nextNode;
        }

        private void setNextNode(SmellNode newNextNode)
        {
            if (newNextNode == null || newNextNode._intensity <= 0)
            {
                return;
            }
            this._nextNode = newNextNode;
        }

        private void setPreviousNode(SmellNode newPreviousNode)
        {
            if (newPreviousNode == null || newPreviousNode._intensity <= 0)
            {
                return;
            }
            this._previousNode = newPreviousNode;                     
            (this._previousNode).setNextNode(this);                   
            (this._previousNode).setIntensity(this._intensity - 1);               
        }                                  


        public int getIntensity()
        {
            return this._intensity;
        }
        private bool setIntensity(int newIntensity)
        {
            // this function goes through all the previous existing nodes and updates their intensity

            if (newIntensity <= 0)
            {
                // If the intensity is <= 0 => remove the node from the chain, if it is in one
                destroySelf();
                return false;
            }
            this._intensity = newIntensity;
            this.updateSpriteColor();

            // if previous node exists (we have a reference to it)
            if (this._previousNode != null)
            {
                this._previousNode.setIntensity(this._intensity - 1);
            }
            return true;
        }

        public void destroySelf()
        {
            // remove from previouses
            if (this._previousNode != null)
            {
                // call recursively to destroy all previous nodes
                this._previousNode.destroySelf();
                
                // remove link to nodes after destroyed
                this._previousNode = null;
            }
            this.destroyObject();

            // remove from nexts
            if (this._nextNode != null)
            {
                // next node link to this
                if (this._nextNode.GetType() == typeof(SmellNode))
                {
                    ((SmellNode)this._nextNode)._previousNode = null;
                }
                else if (this._nextNode.GetType() == typeof(Creature))
                {
                    //((Creature)this._nextNode).deleteSmellReference();
                    //((Creature)this._whose).deleteSmellReference();
                }

                // remove this link to next node
                this._nextNode = null;
            }
            
            // remove reference from grid cell
            GridMap.currentGridInstance.destroySmell(this);
        }

        public System.Type sourceType()
        {
            return this._whose.GetType();
        }
        public LivingEntity source()
        {
            return this._whose;
        }
        public float getDistanceToSource()
        {
            return EcoMath.Math.distanceBetween(new int2(0, 0), sumVectorsToSource());            
        }

        private int2 sumVectorsToSource()
        {
            // returns the resulting vector using polygon summation rule for vectors
            if (this._nextNode.GetType() != typeof(SmellNode))
            {
                // it is the source
                return this.getCoordinates();
            }
            return this.getCoordinates() + ((SmellNode)this._nextNode).sumVectorsToSource();
        }
        public bool isValid()
        {
            if (this._intensity <= 0)
            {
                return false;
            }
            return true;
        }
    }












    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    Smell spreading implementation (more costly, yet more realistic)

    public static class SmellSystem
    {
        private static List<Smell> smells = new List<Smell>();
        public static void executeUpdate()
        {
            if (Configs.Debugging_EnergySystem())
            {
                Debug.Log("Energy System Update");
            }
            updateSmells();
        }

        public static void addSmell(Smell smell)
        {
            smells.Add(smell);
        }
        public static void removeSmell(Smell smell)
        {
            smells.Remove(smell);
        }

        public static void updateSmells()
        {
            foreach(Smell smell in smells)
            {
                smell.dissipate();
                smell.spread();
            }
        }
    }

    
    public abstract class Smell
    {
        protected float _transmissionRate;
        protected float _dissipationRate;
        protected float _smellQuantity;
        protected LivingEntity _whose;


        public Smell(LivingEntity whose)
        {
            this._whose = whose;

            if (whose.GetType() == typeof(Creature))
            {
                Creature creature = (Creature)whose;
                float transmissionFactor = (1 + creature.getGene_FoodPreference()) + (1 + (-1)*creature.getGene_MotorSpeed());
                float dissipationFactor = 0.5f * (1 + (-1)*creature.getGene_FoodPreference()) + (1 + creature.getGene_MotorSpeed());

                // calculate these variables using <whose> <chromosome> stats
                this._transmissionRate = 1 * transmissionFactor;
                this._dissipationRate = 1 * dissipationFactor;

                this._smellQuantity =  10 * creature.getGene_FoodPreference();
            }
            else if (whose.GetType() == typeof(Plant))
            {
                Plant plant = (Plant)whose;
                this._transmissionRate = 2;
                this._dissipationRate = 0.5f;
            }
        }
        public Smell(float transmissionRate, float dissipationRate, LivingEntity whose)
        {
            this._whose = whose;

            this._transmissionRate = transmissionRate;
            this._dissipationRate = dissipationRate;
            this._smellQuantity = this._dissipationRate;
        }


        public float getTransmissionRate()
        {
            return this._transmissionRate;
        }
        public float getDissipationRate()
        {
            return this._dissipationRate;
        }
        public System.Type getEntityType()
        {
            return this._whose.GetType();
        }


        public void spread()
        {
            // code to transmit smell to neighbour cells using GridMap
        }
        public void dissipate()
        {
            this._smellQuantity -= this._dissipationRate;
        }
    }

    public class SmellSource : Smell
    {
        private int2 _coordinates;

        public SmellSource(LivingEntity whose) : base(whose)
        {
            this._coordinates = whose.getCoordinates();
        }
    }

    public class SmellDissipation : Smell 
    {
        private SmellSource _source;

        public SmellDissipation(float transmissionRate, float dissipationRate, LivingEntity whose, SmellSource source) : base(transmissionRate, dissipationRate, whose)
        {
            this._source = source;
        }
    }*/
}   
