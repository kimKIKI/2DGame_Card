using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectingAnimation : MonoBehaviour {

	public enum PLAY_SOUND_MODE { NONE, AT_BEGINNING, AT_THE_END }
    public enum EXPANSION_MODE { UPWARD, EXPLOSION }

    // Factor to adujst upper translation during animation
    [Tooltip("Factor to adujst upper translation during animation")]
	public float _moveUpFactor = 8.0f;
	// Factor to adujst horizontal translation during animation
	[Tooltip("Factor to adujst horizontal translation during animation")]
	public float _moveHorizontalFactor = 3.0f;
	// Factor to adjust scaling of the item while approching destination
	[Tooltip("Factor to adjust scaling of the item while approching destination")]
	public float _scaleDiminutionFactor = 2.0f;
    // Duration of the expansion animation in seconds
    [Tooltip("Duration of the expansion animation in seconds")]
    public float _expansionDuration = 1.0f;
    // Parameter to adujst animation speed
    [Tooltip("Parameter to adujst animation speed")]
	public float _animationSpeed = 2.0f;
	// Reference to the image component of the object
	[Tooltip("Reference to the image component of the object")]
	public Image _image;

	// Flag telling if animation is running or not for this item
	[HideInInspector]
	public bool _animationRunning = false;

	// Reference to the collecting Effect Controller
	private CollectingEffectController _collectinEffectController;
	// 상단아이콘표시위치
	private Transform _itemDisplayerTransform;
	// Reference to the ItemDisplayer component, showing the amount of item collected
	private ItemDisplayer _itemDisplayer;
	// Defines when to play the collecting sound
	private PLAY_SOUND_MODE _playSoundMode;
    // Defines the expansion mode
    private EXPANSION_MODE _expansionMode;

    // destination :collection좌표,parent
    public void Initialize(Transform destination, Transform parent, Vector3 localPosition, Vector3 localScale, PLAY_SOUND_MODE playSoundMode, EXPANSION_MODE expansionMode, CollectingEffectController collectingEffectController)
    {
		_itemDisplayerTransform = destination;
		_itemDisplayer = _itemDisplayerTransform.GetComponent<ItemDisplayer> ();
		transform.SetParent(parent);
		transform.localPosition = localPosition;
		transform.localScale = localScale;

		_playSoundMode = playSoundMode;
        _expansionMode = expansionMode;
        _collectinEffectController = collectingEffectController;
	}

	// Anime의 시작
	public void StartAnimation() {
		_animationRunning = true;
		_image.enabled = true;
		StartCoroutine ("CollectAnimation");
	}

	// Main loop during animation
	IEnumerator CollectAnimation() {
		float t = 0;
		float speed = 1.0f;

		// 첫 실행시 소리발생
		if (_playSoundMode == PLAY_SOUND_MODE.AT_BEGINNING) {			
			_collectinEffectController.PlayCollectingSound ();
		}

        // 1st step :  첫 좌표를 생성
        Vector3 direction;
        if (_expansionMode == EXPANSION_MODE.UPWARD)
        {
            direction = new Vector3((Random.value - 0.5f) * _moveHorizontalFactor, _moveUpFactor, 0.0f);
        } else
        {
            direction = new Vector3((Random.value - 0.5f) * _moveHorizontalFactor, (Random.value - 0.5f) * _moveUpFactor, 0.0f);
        }

		while (t < _expansionDuration)
        {
			t += Time.deltaTime * _animationSpeed;
            if (_expansionMode == EXPANSION_MODE.UPWARD)
            {
                transform.position += Vector3.Scale(direction, new Vector3(1, speed, 1));
            } else {
                transform.position += Vector3.Scale(direction, new Vector3(speed, speed, 1));
            }
			speed = Mathf.Exp(-4*t / _expansionDuration);
			yield return new WaitForFixedUpdate();
		}
		
		// 2nd step : Move to destination
		t = 0;
		Vector3 pos = transform.position;
		Vector3 scale = transform.localScale / _scaleDiminutionFactor;
		while (t < 1.0f)
        {   //보간을 적용해서 pos------->_itemDisplayer의 위치로 이동하게 한다.
			t += Time.deltaTime * _animationSpeed;
			transform.position = Vector3.Lerp(pos, _itemDisplayerTransform.position, t);
			transform.localScale = Vector3.Lerp (transform.localScale, scale, t);
			yield return new WaitForFixedUpdate();
		}

		// 사운드가 움직임이 끝나고 들리게 한다.
		if (_playSoundMode == PLAY_SOUND_MODE.AT_THE_END) {
			_collectinEffectController.PlayCollectingSound ();
		}

		//인스턴스 리스트에 넣고 이미지를 세팅한다.
		_itemDisplayer.AddItem (1);
		_animationRunning = false;
		// Hide this item until next reuse
		_image.enabled = false;
        StartCoroutine(CheckIfAlive(1.5f));
		yield return null;
	}

    IEnumerator CheckIfAlive(float t)
    {
            // 0.5초 간격으로 파티클이 살아 있는지 체크합니다.
            yield return new WaitForSeconds(t);
             GameObject.Destroy(this.gameObject);
           
    }

}
