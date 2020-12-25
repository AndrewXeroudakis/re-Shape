using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour//Singleton<InputManager>
{
	#region Variables
	public readonly static Vector2 DEFAULT_TOUCH_POSITION = new Vector2(-1080, -1920);
	public LayerMask touchInputMask;
	GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;
	EventSystem m_EventSystem;
	#endregion

	#region Unity Event Functions
	protected void Awake()
	{
		//base.Awake();
		m_Raycaster = GetComponent<GraphicRaycaster>();
		m_EventSystem = GetComponent<EventSystem>();
	}
	#endregion

	#region Methods
	public Vector2 GetTouchPosition()
    {
		Vector2 touchPosition = DEFAULT_TOUCH_POSITION;

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
            {
				Vector3 touchPositionVector3 = Camera.main.ScreenToWorldPoint(touch.position);
				touchPosition.x = touchPositionVector3.x;
				touchPosition.y = touchPositionVector3.y;
			}
		}

		return touchPosition;
	}

	public ActionButton GetPressedActionButton() //Vector2 _touchPosition
	{
		m_PointerEventData = new PointerEventData(m_EventSystem);
		m_PointerEventData.position = Input.mousePosition; //_touchPosition; 

		List <RaycastResult> results = new List<RaycastResult>();
		m_Raycaster.Raycast(m_PointerEventData, results);

		foreach (RaycastResult result in results)
		{
			if (result.gameObject.name.Equals("Action Button"))
			{
				ActionButton actionButton = result.gameObject.GetComponent<ActionButton>();
				return actionButton;
			}
		}

		return null;
	}
	#endregion
}
