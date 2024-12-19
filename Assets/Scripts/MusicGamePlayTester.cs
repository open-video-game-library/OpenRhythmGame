using MusicGameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGamePlayTester : MonoBehaviour
{
    [SerializeField] private MusicGamePlayer _musicGamePlayer;
    [SerializeField] private ScoreContainer _scoreContainer;
    [SerializeField] private Button StartButton;

    private void Start()
    {
        StartButton.onClick.AddListener(() => 
        {
            _musicGamePlayer.Play(_scoreContainer);
            StartButton.interactable = false;
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            _musicGamePlayer.Play(_scoreContainer);
        }

        if(Input.GetKeyDown(KeyCode.F1)) _musicGamePlayer.SetHS(false);
        if (Input.GetKeyDown(KeyCode.F2)) _musicGamePlayer.SetHS(true);
    }
}
