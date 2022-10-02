using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    public static class GameObjectHelper 
    {
        public static void RemoveComponent<T>(this GameObject obj,T component) where T : MonoBehaviour
        {
            if(obj.GetComponent<T>() == null)
            {
                return;
            }
            T[] ts = obj.GetComponents<T>();
            if(ts == null || ts.Length == 0)
            {
                return;
            }
            for(int i = 0; i < ts.Length; i++)
            {
                if (ts[i] != component)
                {
                    continue;
                }
                GameObject.Destroy(component);
            }
        }
    }
}
