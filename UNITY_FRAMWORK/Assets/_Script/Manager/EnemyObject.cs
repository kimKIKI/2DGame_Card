using UnityEngine;
using System.Collections;

public class EnemyObject : MonoBehaviour
{
	//-------------------------------------------------------
	//C# accessors for private variables
	public int Health
	{
		get{return _health;}
		set
		{
			//Clamp health between 0-100
			_health = Mathf.Clamp(value, 0, 100);

			//Post notification - health has been changed
			EventManager.Instance.PostNotification(EVENT_TYPE.HEALTH_CHANGE, this, _health);
		}
	}
	//-------------------------------------------------------
	public int Ammo
	{
		get{return _ammo;}
		set
		{
			//Clamp ammo between 0-50
			_ammo = Mathf.Clamp(value,0,50);

			//Post notification - ammo has been changed
			EventManager.Instance.PostNotification(EVENT_TYPE.AMMO_CHANGE, this, _health);
		}
	}
	//-------------------------------------------------------
	//Internal variables for health and ammo
	private int _health = 100;
	private int _ammo = 50;
	//-------------------------------------------------------
	//Called at start-up
	void Start()
	{
		//Add myself as listener for health change events
		EventManager.Instance.AddListener(EVENT_TYPE.HEALTH_CHANGE, OnEvent);
	}
	//-------------------------------------------------------
	// Update is called once per frame
	void Update () 
	{
		//If you press space bar, the health is reduce
		if(Input.GetKeyDown(KeyCode.Z))
		{
			//Take some damage of space bar  press
			Health -= 5;
		}
	}
	//-------------------------------------------------------
	//Called when events happen
	public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
	{
		//Detect event type
		switch(Event_Type)
		{
			case EVENT_TYPE.HEALTH_CHANGE:
				OnHealthChange(Sender, (int)Param);
			break;
		}
	}
	//-------------------------------------------------------
	//Function called when health changes
	void OnHealthChange(Component Enemy, int NewHealth)
	{
		//If health has changed of this object
		if(this.GetInstanceID() != Enemy.GetInstanceID()) return;

		Debug.Log ("Object: " + gameObject.name +  " Health is: " + NewHealth.ToString());
	}
	//-------------------------------------------------------
}
