using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using File = System.IO.File;

/// <summary>
/// 게임 내의 인스턴스 데이터(씬 등에서 일시적으로 사용하는 데이터) 외의 데이터를 저장하는 구조
/// 즉 게임 한편 인스턴스에서 저장하는 데이터를 가지고 있다.
/// </summary>
public class GameCommon : ScriptableObject
{
    private static string filepath = "1111.tmp";
    
    private static GameCommon instance;
    public static GameCommon Instance
    {
        get
        {
            if(!instance)
            {
                instance = CreateInstance<GameCommon>();
            }
            return instance;
        }
    }

    public Difficulty SelectedDifficulty;

    public PlayerCharacter SelectedCharacter;

    public int Exp;

    public int GainExp(int value)
    {
        Exp += value;
        Logger.DebugLog(string.Format("GainExp : {0}", value));
        return Exp;
    }

    // 스킬셋 리스트 어떻게 만들까
    // 딕셔너리? 어레이? 리스트?

    // 지금까지 돌았던 맵

    private void Awake()
    {
        
    }

    // 지정된 파일 경로에 저장하기
    public static void SaveStatus()
    {
        var JsonData = JsonUtility.ToJson(instance);
        File.WriteAllBytes(filepath, System.Text.Encoding.UTF8.GetBytes(JsonData));
    }
    
    // 지정된 파일 경로에서 읽어오기
    public static bool LoadStatus()
    {
        if (!File.Exists(filepath))
        {
            return false;
        }
        else
        {
            try
            {
                var JsonObject = JsonUtility.FromJson<GameCommon>(File.ReadAllBytes(filepath).ToString());
                instance = JsonObject;
            }
            catch (System.Exception e)
            {
                Logger.DebugLog(e.Message);
                // UI에도 오류임을 표시해야한다.
                return false;
            }
        }
        return true;
    }

    // 정상 종료된 경우 임시 세이브 없이 삭제한다.
    public static void DeleteStatus()
    {
        File.Delete(filepath);
    }
}

 // 선택 캐릭터
public enum PlayerCharacter
{
    Player1, // 플레이어 1 (임시 더미)
}

// 난이도
public enum Difficulty
{
    Easy,       // 적 체력 50%
    Normal,     // 적 체력 100%
    Hard,       // 적 체력 200%, 공격력 150%
    VeryHard,   // 적이 더 나옴, 적 체력 250%, 공격력 150%
    Crusaders   // 적이 더 나옴, 특수 패턴 추가, 적 체력 300%, 공격력 175%
}