using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using File = System.IO.File;

/// <summary>
/// 게임의 전체 설정을 저장하는 구조.
/// 플레이 인스턴스 외에 유지되는 정보들을 저장한다.
/// </summary>
public class CommonConfig : ScriptableObject
{
    private static string configPath = @"Config.ini";

    public string UserName = "Player 1";

    // 해금 요소
    public List<PlayerCharacter> AvailableCharacters = new List<PlayerCharacter>() { PlayerCharacter.Player1 };
    public List<Difficulty> AvailableDifficulty = new List<Difficulty>() { Difficulty.Easy, Difficulty.Normal, Difficulty.Hard };

    private static CommonConfig instance;
    public static CommonConfig Instance
    {
        get
        {
            if (!instance)
            {
                instance = CreateInstance<CommonConfig>();
            }
            return instance;
        }
    }

    private void Awake()
    {

    }

    // 지정된 파일 경로에 저장하기
    public static void SaveConfig()
    {
        var JsonData = JsonUtility.ToJson(instance);
        File.WriteAllBytes(configPath, System.Text.Encoding.UTF8.GetBytes(JsonData));
    }

    // 지정된 파일 경로에서 읽어오기
    public static bool LoadConfig()
    {
        if (!File.Exists(configPath))
        {
            ResetConfig();
        }
        else
        {
            try
            {
                var JsonObject = JsonUtility.FromJson<CommonConfig>(File.ReadAllBytes(configPath).ToString());
                instance = JsonObject;
            }
            catch ( System.Exception e)
            {
                Logger.DebugLog(e.Message);
                // UI에도 오류임을 표시해야한다.
                return false;
            }
        }
        return true;
    }

    // 디폴트값 생성
    public static void ResetConfig()
    {
        instance = CreateInstance<CommonConfig>();
        SaveConfig();
    }
}
