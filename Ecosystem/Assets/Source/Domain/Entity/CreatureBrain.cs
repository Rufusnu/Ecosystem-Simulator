using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfigDomain;

namespace EntityDomain
{
    public class CreatureBrain : MonoBehaviour
    {
        // #### [++] Attributes [++] ####
        private float _time;
        private Creature _creature;
        // #### [--] Attributes [--] ####


        // Start is called before the first frame update
        void Start()
        {
            this._time = 0;
        }


        // #### [++] Updates [++] ####
        // Update is called once per frame
        void Update()
        {
            executeEvery(GameConfig.instance.UpdateIntervalOfCreatureBrain);
        }
        private void executeEvery(float seconds)
        {
            // execute what needs to be executed every given seconds
            this._time += Time.deltaTime;
            if (this._time >= seconds)
            {
                this._creature.brainUpdate(); // call updater in creature's object
                this._time = 0;
            }
        }
        // #### [--] Updates [--] ####

        public void setCreature(Creature newCreature)
        {
            this._creature = newCreature;
        }
    }

}
