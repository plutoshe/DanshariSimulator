using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GraphicsCursor : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    GameObject selectObject;
    GameObject oldSelectObject;
    Vector3 selectOffset;
    Vector3 oldPosition;
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("PlayerItem") && !result.gameObject.GetComponent<PlayerItemBehavious>().finished)
                {
                    selectObject = result.gameObject;
                    selectOffset = selectObject.transform.position - Input.mousePosition;
                    
                    
                    //timelineUI.editingBulletStatus.SetSelectOffset(m_PointerEventData.position);
                }
            }
            if (oldSelectObject && oldSelectObject != selectObject)
                oldSelectObject.GetComponent<PlayerItemBehavious>().CancelShow();
            oldPosition = selectObject.transform.position;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (selectObject)
                selectObject.transform.position = selectOffset + Input.mousePosition;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            oldSelectObject = selectObject;
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            bool isDestroy = false;
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Trash")
                {
                    GameStatus.Instance.playerItems.Remove(selectObject.name.ToLower());
                    Destroy(selectObject);
                    isDestroy = true;
                    oldSelectObject = null;
                    break;
                }

               
            }
            
            if (!isDestroy)
                if (selectObject && oldPosition == selectObject.transform.position)
                    selectObject.GetComponent<PlayerItemBehavious>().OnClick();

            selectObject = null;
            
        }
    }
}