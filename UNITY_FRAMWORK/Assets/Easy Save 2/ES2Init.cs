using System; 
using UnityEngine; 
using System.Collections; 
using System.Collections.Generic;

[ExecuteInEditMode]				
public class ES2Init : MonoBehaviour
{
	public void Awake()
	{
		Init();
	}
	
	public void Start()
	{
		if(Application.isEditor)
			GameObject.DestroyImmediate(gameObject);
		else
			GameObject.Destroy(gameObject);
	}

	public static void Init()
	{
		ES2TypeManager.types = new Dictionary<Type, ES2Type>();
				ES2TypeManager.types[typeof(UnityEngine.AudioClip)] = new ES2_AudioClip();
		ES2TypeManager.types[typeof(UnityEngine.GameObject)] = new ES2_GameObject();
		ES2TypeManager.types[typeof(UnityEngine.Texture2D)] = new ES2_RawTexture2D();
		ES2TypeManager.types[typeof(UnityEngine.CanvasRenderer)] = new ES2UserType_UnityEngineCanvasRenderer();
		ES2TypeManager.types[typeof(UnityEngine.LineRenderer)] = new ES2UserType_UnityEngineLineRenderer();
		ES2TypeManager.types[typeof(UnityEngine.RectTransform)] = new ES2UserType_UnityEngineRectTransform();
		ES2TypeManager.types[typeof(UnityEngine.UI.Text)] = new ES2UserType_UnityEngineUIText();
		ES2TypeManager.types[typeof(UnityEngine.UI.Outline)] = new ES2UserType_UnityEngineUIOutline();

		ES2.initialised = true;
	}
}