using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPooling
{
    public class BulletsPool : ObjectPool<BulletTypes, MonoBehaviour>
    {
        public static BulletsPool current;
        private void Awake()
        {
            current = this;
        }
    }

    public enum BulletTypes : byte
    {
        Zn1Shoot,
        Zn1ImpactEnemy,
        Zn1ImpactWall,

        SpiderExplosion,
    }
}
