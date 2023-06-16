using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GemSpawn : MonoBehaviour
{
    public List<Transform> gems = new List<Transform>();
    public bool collected=false;
  //  int childCount;
    
  
    public void Spawn()
    {
        
        Transform tr = gems[Random.Range(0, gems.Count)];   
        GameObject go = Instantiate(tr.gameObject,tr.transform.position,Quaternion.identity);
       
        
        go.transform.DOScale(Vector3.one,5).SetEase(Ease.OutExpo).OnUpdate(() =>
        {
            if (go.transform.localScale.x >= 0.25f)
            {
                
                    if (go.GetComponent<BoxCollider>())
                    {
                        go.GetComponent<BoxCollider>().enabled = true;
                    }
                                              

            }
            
        }); 

    }

}
