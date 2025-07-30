using System.Collections.Generic;
using System.Linq;
using Gameplay;
using UnityEngine;

namespace ObjectPool
{
    public class FoodFactory : MonoBehaviour
    {
        public static FoodFactory instance;

        public Plato[] foodPrefabs;
        public Dictionary<Plato, ObjectPool<Plato>> foodPools = new Dictionary<Plato, ObjectPool<Plato>>();
        public int stonks = 15;
        public bool dynamic = true;

        void Awake()
        {
            instance = this;
            foreach (var p in foodPrefabs)
            {
                if (!foodPools.ContainsKey(p))
                {
                    // Crear un pool si no existe una para esa comida
                    foodPools[p] = new ObjectPool<Plato>(() => FoodCreator(p), Plato.TurnOnOff, stonks, dynamic);
                }
            }
        }

        private Plato FoodCreator(Plato p)
        {
            return Instantiate(p, transform);
        }

        public Plato GetFood(Plato p)
        {
            if (!foodPools.ContainsKey(p))
            {
                foodPools[p] = new ObjectPool<Plato>(() => FoodCreator(p), Plato.TurnOnOff, stonks, dynamic);
            }

            return foodPools[p].GetObject();
        }

        public Plato GetRandomFood()
        {
            return foodPools[foodPrefabs[Random.Range(0, foodPrefabs.Length)]].GetObject();
        }

        public void ReturnFood(Plato p)
        {
            foreach (var poolEntry in foodPools.Where(poolEntry => poolEntry.Key.GetType() == p.GetType()))
            {
                poolEntry.Value.ReturnObject(p);
                return;
            }
        }
    }
}
