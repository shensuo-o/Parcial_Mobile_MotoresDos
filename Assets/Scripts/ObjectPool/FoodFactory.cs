using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFactory : MonoBehaviour
{
    public static FoodFactory Instance;

    public Plato[] foodPrefabs;
    public Dictionary<Plato, ObjectPool<Plato>> foodPools = new Dictionary<Plato, ObjectPool<Plato>>();
    public int stonks = 15;
    public bool dynamic = true;

    void Start()
    {
        Instance = this;
        foreach (Plato p in foodPrefabs)
        {
            if (!foodPools.ContainsKey(p))
            {
                // Crear un pool si no existe una para esa comida
                foodPools[p] = new ObjectPool<Plato>(() => FoodCreator(p), Plato.TurnOnOff, stonks, dynamic);
            }
        }
    }

    public Plato FoodCreator(Plato p)
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
        // Encontrar la pool y traer el enemigo
        foreach (var poolEntry in foodPools)
        {
            if (poolEntry.Key.GetType() == p.GetType())
            {
                poolEntry.Value.ReturnObject(p);
                return;
            }
        }
    }
}
