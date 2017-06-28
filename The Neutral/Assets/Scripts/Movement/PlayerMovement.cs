using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public static class PlayerMovement
    {
        private static Dictionary<string, UnityEngine.AI.NavMeshAgent> recentControlCharacters;
        public static UnityEngine.AI.NavMeshAgent agent { get; set; }

        private static bool debug = false;
        private static bool isPlayerControl;

        public static void SetInitialMovement()
        {
            agent = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityEngine.AI.NavMeshAgent>();
            recentControlCharacters = new Dictionary<string, UnityEngine.AI.NavMeshAgent>();
            recentControlCharacters.Add("Player", agent);
            isPlayerControl = true;
            agent.updateRotation = false;
        }

        public static bool inControl(bool isPlayer)
        {
            return (isPlayerControl == isPlayer);
        }

        public static int Move()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.GetPoint(500), Color.red);
            if (Input.GetMouseButtonDown(0))
            {
				if (Physics.Raycast(ray, out hit, 500, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.CompareTag("Player-Sphere"))
                    {
                        isPlayerControl = true;
                        if (recentControlCharacters.ContainsKey("Player"))
                        {
                            agent = recentControlCharacters["Player"];
                        }
                        else
                        {
                            recentControlCharacters.Add("Player", GameObject.FindGameObjectWithTag("Player").GetComponent<UnityEngine.AI.NavMeshAgent>());
                            agent = recentControlCharacters["Player"];
                        }
                            
                    }
                    else if (hit.collider.CompareTag("Enemy-Sphere"))
                    {
                        isPlayerControl = false;
                        if (recentControlCharacters.ContainsKey("Enemy"))
                        {
                            agent = recentControlCharacters["Enemy"];
                        }
                        else
                        {
                            recentControlCharacters.Add("Enemy", GameObject.FindGameObjectWithTag("Enemy").GetComponent<UnityEngine.AI.NavMeshAgent>());
                            agent = recentControlCharacters["Enemy"];
                        }
                            
                    }
                    
                    else 
                    {
                        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

                        bool hasFoundPath = agent.CalculatePath(hit.point, path);
                        if (debug) Debug.Log(agent.updateRotation);
                        Debug.Log(hit.point);

                        switch (path.status)
                        {
                            case UnityEngine.AI.NavMeshPathStatus.PathComplete:
                                agent.SetPath(path);
                                return (int)UnityEngine.AI.NavMeshPathStatus.PathComplete;
                            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.point), Time.deltaTime);

                            case UnityEngine.AI.NavMeshPathStatus.PathPartial:
                                agent.SetPath(path);
                                Debug.Log("PARTIAL PATH");
                                return (int)UnityEngine.AI.NavMeshPathStatus.PathPartial;

                            case UnityEngine.AI.NavMeshPathStatus.PathInvalid:
                                Debug.Log("INVALID PATH");
                                return (int)UnityEngine.AI.NavMeshPathStatus.PathInvalid;
                        }
                    }
                    

                } //end raycast

                else
                {
                    Debug.Log("NOT HIT");
                }
            } //end mousebuttondown(0)

            return -1;
        } //end move
    }

}