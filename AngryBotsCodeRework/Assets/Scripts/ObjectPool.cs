using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPooling
{
    public abstract class ObjectPool<E, T> : MonoBehaviour
        where T : MonoBehaviour where E : Enum
    {
        [SerializeField] BulletPrototype[] prefabs;
        private Dictionary<E, List<T>> objects = new Dictionary<E, List<T>>();
        public T Take(E objectType, Vector3 pos, Quaternion rot)
        {
            if (!objects.ContainsKey(objectType))
            {
                objects.Add(objectType, new List<T>());
            }

            var subPool = objects[objectType];
            foreach (T obj in subPool)
            {
                if (!obj.gameObject.activeSelf)
                {
                    Transform objTr = obj.transform;
                    objTr.position = pos;
                    objTr.rotation = rot;
                    objTr.SetParent(transform);
                    objTr.gameObject.SetActive(true);
                    return obj;
                }
            }

            var currentPrefab = prefabs.First(p => p.type.Equals(objectType)).prefab;

            MonoBehaviour newObj = Instantiate<MonoBehaviour>(currentPrefab) as MonoBehaviour;
            subPool.Add((T)newObj);
            Transform newObjTr = newObj.transform;
            newObjTr.SetParent(transform);
            newObjTr.position = pos;
            newObjTr.rotation = rot;
            newObjTr.gameObject.SetActive(true);
            newObjTr.gameObject.name += subPool.Count;

            return (T)newObj;
        }

        public void Clear()
        {
            foreach (List<T> subPool in objects.Values)
            {
                foreach (var obj in subPool)
                {
                    Destroy(obj.gameObject);
                }
                subPool.Clear();
            }
            objects.Clear();
        }
    }

    [Serializable]
    public class Prototype<E, T> where E : Enum where T : MonoBehaviour
    {
        [SerializeField] public E type;
        [SerializeField] public T prefab;
    }

    [Serializable]
    public class BulletPrototype : Prototype<BulletTypes, MonoBehaviour>
    {
    }
}
