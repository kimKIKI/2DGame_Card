using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class zFoxGameObjectLoader : MonoBehaviour {
	//외부 파라미터 
	public GameObject [] LoadGameObjectList_Awake;
	public GameObject [] LoadGameObjectList_Start;
	public GameObject [] LoadGameObjectList_Update;
	public GameObject [] LoadGameObjectList_FixedUpdate;

	[System.NonSerialized] public Dictionary<string,GameObject>  loadedGameObjectList_Awake = new Dictionary<string,GameObject>();
	[System.NonSerialized] public bool loaded_Awake = false;
	[System.NonSerialized] public Dictionary<string,GameObject>  loadedGameObjectList_Start = new Dictionary<string,GameObject>();
	[System.NonSerialized] public bool loaded_Start = false;
	[System.NonSerialized] public Dictionary<string,GameObject>  loadedGameObjectList_Update = new Dictionary<string,GameObject>();
	[System.NonSerialized] public bool loaded_Update = false;
	[System.NonSerialized] public Dictionary<string,GameObject>  loadedGameObjectList_FixedUpdate = new Dictionary<string,GameObject>();
	[System.NonSerialized] public bool loaded_FixedUpdate = false;

	bool loaded =false;

	void Awake(){
		//각 게임이 로드 됐는지 검사 
		bool loadedAll = false;
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject  go in gos){
			zFoxGameObjectLoader  fol = go.GetComponent<zFoxGameObjectLoader>();
			if(fol){
				if(fol.loaded){
					loadedAll= true;
					break;
				}
			}
		}
		if(loadedAll){
			Destroy(gameObject);
			return;
		}
		loaded = true;
		//Awke 처리 실행
		if(!loaded_Awake){
			loaded_Awake = true;
			LoadGameObject(LoadGameObjectList_Awake,loadedGameObjectList_Awake);
		}
	}
	void Start(){
		if(!loaded_Start){
			loaded_Start = true;
			LoadGameObject(LoadGameObjectList_Start,loadedGameObjectList_Start);
		}
	}
	void Update(){
		if(!loaded_Update){
			loaded_Update= true;
			LoadGameObject(LoadGameObjectList_Update,loadedGameObjectList_Update);
		}
	}
	void FixedUpdate(){
		if(!loaded_FixedUpdate){
			loaded_FixedUpdate = true;
			LoadGameObject(LoadGameObjectList_FixedUpdate,loadedGameObjectList_FixedUpdate);
		}
	}
	void LoadGameObject(GameObject[] loadGameObjectList,Dictionary<string,GameObject> LoadGameObjectList){
		//로더가 씬 전환때 삭제되지 안도록 한다 
		//로드할 게임 오브젝트는 자식에게 설정되므로 로드된것도 삭제되지 않는다 
		DontDestroyOnLoad(this);
		//등록되어있는 게임 오브젝트를 불러 온다 
		foreach(GameObject go in loadGameObjectList){
			if(go){
				if(LoadGameObjectList.ContainsKey(go.name)){

				}else{
					GameObject goInstance = Instantiate(go) as GameObject;
					goInstance.name = go.name;
					goInstance.transform.parent = gameObject.transform;
					LoadGameObjectList.Add (go.name,goInstance);
					Debug.Log (string.Format("Loaded GameOject{0}",go.name));
				}
			}
		}
	}
}
