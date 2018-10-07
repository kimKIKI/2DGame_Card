using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class CollectingEffectController : MonoBehaviour {

	// Play collecting sound at begining of the animation or at the end
	[Tooltip("Play collecting sound at begining of the animation or at the end ?")]
	public CollectingAnimation.PLAY_SOUND_MODE _playSoundMode;
    [Tooltip("Define the expansion animation mode")]
    public CollectingAnimation.EXPANSION_MODE _expansionMode = CollectingAnimation.EXPANSION_MODE.UPWARD;
    // 아이템이 생성되는 간격
    [Tooltip("The emission rate in seconds")]
	public float _emissionRate = 0.2f;
	// The tranform component of the item displayer
	[Tooltip("The tranform component of the item displayer")]
	public Transform _itemDisplayer;
	// The position where to pop the items
	[Tooltip("The position where to pop the items")]
	public Transform _popPosition;
	// The prefab of the items to instanciate
	[Tooltip("The prefab of the items to instanciate")]
	public GameObject _itemPrefab;
    [HideInInspector]
    public int Amount; //prefab에 개당 value를 전달하기 위해서 사용.
	// Instance of this class
	[HideInInspector]
	public static CollectingEffectController _instance;

	// This is a list of instanciated _itemPrefab 
	private List<CollectingAnimation> _itemList = new List<CollectingAnimation>();
	// Reference to the AudioSource component
	//private AudioSource _audioSource;


	void Awake() {
		
		_instance = this;
		//_audioSource = GetComponent<AudioSource> ();
	}

	// Collect some items with animation
	public void CollectItem(int quantity) {
		StartCoroutine (PopItems(quantity));
	}

	// Collect some items with animation at a fixed position
	public void CollectItemAtPosition(int quantity, Vector3 position) {
		// Set the position
		_popPosition.position = position;
		StartCoroutine (PopItems(quantity));
	}

    //quantity :스폰될 갯수_emissionRate :스폰간격
    //Here we pop all the necessary items
    IEnumerator PopItems(int quantity) {
		WaitForSeconds delay = new WaitForSeconds (_emissionRate);
		for (int i = 0; i < quantity; i++) {
			CollectingAnimation animation = null;
			if(i < _itemList.Count) {
				if(!_itemList[i]._animationRunning) {
					// A free object has been found in pool, so we reuse it
					animation = _itemList[i];
				}
			} 
			if(animation == null) {
				// No free object has been found in pool, so we instantiate a new one
				GameObject go = Instantiate (_itemPrefab) as GameObject;
				animation = go.GetComponent<CollectingAnimation>();
				_itemList.Add(animation);
			}

			// Initialize object
			animation.Initialize(_itemDisplayer, _popPosition, Vector3.zero, Vector3.one, _playSoundMode, _expansionMode,Amount, this);
			// Start animation
			animation.StartAnimation();
			yield return delay;
		}
	}

	// Play the collecting sound
	public void PlayCollectingSound() {
        //_audioSource.Play ();
        AppSound.instance.SE_ITEM_COIN.Play();
	}
}
