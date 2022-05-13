using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public class Plant : LivingEntity
    {
        public static int plantCounter = 0;
        private float _age;


        public Plant(int2 newCoordinates) : base(newCoordinates)
        {
            initializePlantObject();
            initializeNutritionValue();
            initializeAge();
            Plant.plantCounter++;
        }
        private void initializePlantObject()
        {
            this.setObject(new GameObject("[Plant" + Plant.plantCounter + "]"));
            this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.plant_default;
            this.getObject().transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            this.getObject().GetComponent<SpriteRenderer>().color = new Color(0.15f, 1f, 0.15f, 1.0f);      // green color
        }
        protected override void initializeNutritionValue()
        {
            // value [-20%; +20%] * Average Nutrition Value (Default value from entity config)
            float min = EntityConfig.instance.PlantMinNutritionValue;
            float max = EntityConfig.instance.PlantMaxNutritionValue;

            this._nutritionValue = (float)UnityEngine.Random.Range(min, max);
        }
        private void initializeAge()
        {
            this._age = 0.1f;
        }

        protected override void eat(LivingEntity entity)
        {
            // TO DO
        }
        public override void eaten()
        {
            this.die();
        }
    }
}

