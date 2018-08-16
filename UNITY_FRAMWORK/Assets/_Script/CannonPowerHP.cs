using UnityEngine;
using System.Collections;

public class CannonPowerHP : MonoBehaviour {

	private float mPowerHp;
	private float mNowpower;
	public Transform mBar; //HP bar

	int Max = 800;

	public void SetBar(int power){

		Vector3 originalScale = new  Vector3(5f,5f,5f);
		mBar.transform.localScale =  new Vector3 (originalScale.x*power/Max,5,5);

	}

	public void Invisible(bool isB)
	{
		// 탑승 상태가 아닐때 안보임 
		gameObject.SetActive(isB);	
	}
 
}
