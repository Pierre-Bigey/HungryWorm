using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    public class SliceItemPool
    {
        public List<GameObject> pool;
        public GameObject prefab;
        public int initialPoolSize;
        public int minAmountToSpawn;
        public int maxAmountToSpawn;
    }
}