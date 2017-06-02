using UnityEngine;
using System.Collections;


namespace Neutral
{
    public class GenerateNature : MonoBehaviour
    {

        public GameObject TreeDeciA;
        public GameObject TreeDeciB;

        public GameObject LogA;
        public GameObject LogB;
        public GameObject LogC;


        // Use this for initialization
        void Start()
        {
            for (int x=0; x<100; x++)
            {
                Instantiate(TreeDeciA, new Vector3(Random.Range(-600, 600),30, Random.Range(-600, 600)), this.TreeDeciA.transform.rotation);
                Instantiate(TreeDeciB, new Vector3(Random.Range(-600, 600), 30, Random.Range(-600, 600)), this.TreeDeciA.transform.rotation);
                //Instantiate(LogA, new Vector3(Random.Range(-600, 600), Random.Range(10, 24), Random.Range(-600, 600)), this.TreeDeciA.transform.rotation);
                //Instantiate(LogB, new Vector3(Random.Range(-600, 600), Random.Range(10, 24), Random.Range(-600, 600)), this.TreeDeciA.transform.rotation);
                //Instantiate(LogC, new Vector3(Random.Range(-600, 600), Random.Range(10, 24), Random.Range(-600, 600)), this.TreeDeciA.transform.rotation);
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}