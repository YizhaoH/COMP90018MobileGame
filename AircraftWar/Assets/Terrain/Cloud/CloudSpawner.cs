using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YizhaoCode.Cloud;


namespace YizhaoCode.Cloud
{
    
public class CloudSpawner : MonoBehaviour
{
        public GameObject cloudPrefab01;
        public GameObject cloudPrefab02;
        public GameObject cloudPrefab03;
        
        public int minHeight = 250;
        public int maxHeight = 500;
        public int numCloud01 = 100;
        public int numCloud02 = 75;
        public int numCloud03 = 50;

        void Start()
        {
            Spawn(cloudPrefab01, numCloud01);
            Spawn(cloudPrefab02, numCloud02);
            Spawn(cloudPrefab03, numCloud03);

        }

        void Spawn(GameObject prefab, int num)
        {
            if (prefab==null || num<=0) return;
            for (int i = 0; i < num; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0, 3000), Random.Range(minHeight, maxHeight), Random.Range(0, 3000));
                GameObject newObject = Instantiate(prefab, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0))) as GameObject;  // instatiate the object
                float randomScale = Random.Range(6, 12);
                newObject.transform.localScale = new Vector3(randomScale,  randomScale, randomScale); // change its local scale in x y z format
            }
        }

    }

}
