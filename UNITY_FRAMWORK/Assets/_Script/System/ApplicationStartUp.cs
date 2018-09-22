using UnityEngine;
using System.Collections;

public class ApplicationStartUp : MonoBehaviour {


    private void Awake()
    {
        Debug.Log("시작과 동시에 게임이 끝날때까지 실행되어야 할것들");
        SaveData.LoadOption();
        //파일 파싱
        //SpriteManager 먼저 로드 되어야 한다.
        SpriteManager.Load("Sprite");
        FileDataManager.Instance.ParsingFirstUnitData();
        FileDataManager.Instance.ParsingPlayerData();
        FileDataManager.Instance.ParsingMarket();
    }
    
}
