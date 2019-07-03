using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{

    #region ScoreManager
    public static int CurMusicScore { get; set; }
    public static int Combo { get; set; }
    /// <summary>
    /// 最大连击数
    /// </summary>
    public static int CurMaxCombo { get; set; }

    private const int PerfectValue = 1000;
    private const int GoodValue = 800;
    private const int BadValue = 0;

    public static void ResetScore()
    {
        CurMusicScore = 0;
    }

    public static void ResetCombo()
    {
        Combo = 0;
    }

    /// <summary>
    /// 累计分值
    /// </summary>
    /// <param name="state"></param>
    public static void SetScoreByHitStateType(HitState state)
    {
        switch (state)
        {
            case HitState.Perfect:
                CurMusicScore += PerfectValue;
                Combo++;
                break;
            case HitState.Good:
                CurMusicScore += GoodValue;
                Combo++;
                break;
            case HitState.Bad:
                CurMusicScore += BadValue;
                if(Combo >= CurMaxCombo)
                    CurMaxCombo = Combo;
                Combo = 0;
                break;
            default:
                break;
        }

        ComboView.instance.ShowScore();
        ScoreView.instance.ShowScore();

        MusicStepEffect.instance.CreateEffect(Combo);
    }
 
    /// <summary>
    /// 储存分值
    /// </summary>
    /// <param name="musicName"></param>
    public static void SaveMusicData(string musicName)
    {
        if (CurMusicScore >= GetScore(musicName))
            SetScore(musicName);
        if (Combo >= GetCombo(musicName))
            SetCombo(musicName);
    }

    /// <summary>
    /// 获取记录 分值
    /// </summary>
    /// <param name="musicName"></param>
    /// <returns></returns>
    public static int GetScore(string musicName)
    {
        return PlayerPrefs.GetInt(musicName + "Score");
    }

    public static void SetScore(string musicName)
    {
        PlayerPrefs.SetInt(musicName + "Score",CurMusicScore);
    }
    /// <summary>
    /// 获取记录连击数
    /// </summary>
    /// <param name="musicName"></param>
    /// <returns></returns>
    public static int GetCombo(string musicName)
    {
        return PlayerPrefs.GetInt(musicName + "Combo");
    }

    public static void SetCombo(string musicName)
    {
        PlayerPrefs.SetInt(musicName + "Combo", CurMaxCombo);
    }
    #endregion
}
