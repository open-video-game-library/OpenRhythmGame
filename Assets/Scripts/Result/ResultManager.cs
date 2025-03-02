using System;
using System.Collections;
using System.Collections.Generic;
using HolmonUtility;
using MusicGameEngine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour, IOnLoadedReaction
{
    //パラメータ
    //------------------------------------------------------------------------------------
    [Header("システム参照")]
    [SerializeField] private LoadSceneUtility _loadSceneUtility;
    
    [Header("リザルトの表示")]
    [SerializeField] private TMP_Text _songTitleText;
    [SerializeField] private TMP_Text _artistText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _perfectText;
    [SerializeField] private TMP_Text _goodText;
    [SerializeField] private TMP_Text _badText;
    [SerializeField] private TMP_Text _missText;

    [Header("インタラクション")]
    [SerializeField] private Button _nextButton;
    
    //パブリックメソッド
    //------------------------------------------------------------------------------------
    
    public void ReceiveOnLoaded(params object[] iSceneSharedObjects)
    {
        var scoreContainer = (ScoreContainer)iSceneSharedObjects[0];
        var pointData = (PointData)iSceneSharedObjects[1];
        
        DisplayResult(scoreContainer, pointData);
    }

    public void NextScene()
    {
        Debug.Log("次のシーンへの呼び出しが行われました");
        _loadSceneUtility.LoadScene("PlayScene_Demo");
    }
    
    //プライベートメソッド
    //------------------------------------------------------------------------------------

    private void Start()
    {
        _nextButton.onClick.AddListener(NextScene);
    }

    private void DisplayResult(ScoreContainer score, PointData point)
    {
        var songTitle = score.SongName;
        var composer = score.ArtistName;
        var scoreValue = point.point;
        var rank = point.rank;
        var perfect = point.perfect;
        var good = point.good;
        var bad = point.bad;
        var miss = point.miss;
        
        _songTitleText.text = songTitle;
        _artistText.text = composer;
        _scoreText.text = scoreValue.ToString();
        _rankText.text = rank.ToString();
        _perfectText.text = perfect.ToString();
        _goodText.text = good.ToString();
        _badText.text = bad.ToString();
        _missText.text = miss.ToString();
    }
}