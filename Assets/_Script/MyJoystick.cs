using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    [HideInInspector]
    public bool pressed;
   
  
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Pressed");
        pressed = true;
           
        
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Released");
        pressed = false;
           
    }
    

}
