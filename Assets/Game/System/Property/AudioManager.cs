// 日本語対応
using System.Collections.Generic;
using UniRx;
using CriWare;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

[Serializable]
public class AudioManager
{
    [SerializeField]
    private FloatReactiveProperty _masterVolume = new FloatReactiveProperty(1f);
    [SerializeField]
    private FloatReactiveProperty _bgmVolume = new FloatReactiveProperty(1f);
    [SerializeField]
    private FloatReactiveProperty _seVolume = new FloatReactiveProperty(1f);

    private CriAtomExPlayer _bgmPlayer = new CriAtomExPlayer();
    private CriAtomExPlayback _bgmPlayback;
    private CriAtomExPlayer _sePlayer = new CriAtomExPlayer();
    private CriAtomExPlayer _loopSEPlayer = new CriAtomExPlayer();
    private List<CriPlayerData> _seData = new List<CriPlayerData>();
    private string _currentBGMCueName = "";
    private CriAtomExAcb _currentBGMAcb = null;

    public IReadOnlyReactiveProperty<float> MasterVolume => _masterVolume;
    public IReadOnlyReactiveProperty<float> BGMVolume => _bgmVolume;
    public IReadOnlyReactiveProperty<float> SEVolume => _seVolume;

    private const string SaveFileName = "AudioVolume";

    /// <summary>SEのPlayerとPlaback</summary>
    struct CriPlayerData
    {
        private CriAtomExPlayback _playback;
        private CriAtomEx.CueInfo _cueInfo;


        public CriAtomExPlayback Playback
        {
            get => _playback;
            set => _playback = value;
        }
        public CriAtomEx.CueInfo CueInfo
        {
            get => _cueInfo;
            set => _cueInfo = value;
        }

        public bool IsLoop
        {
            get => _cueInfo.length < 0;
        }
    }

    public void Save()
    {
        SaveLoadManager.Save<AudioManager>(this, SaveFileName);
    }
    public void Load()
    {
        var tmp = SaveLoadManager.Load<AudioManager>(SaveFileName);
        if (tmp == null)
        {
            return;
        }
        this._masterVolume.Value = tmp._masterVolume.Value;
        this._bgmVolume.Value = tmp._bgmVolume.Value;
        this._seVolume.Value = tmp._seVolume.Value;
    }

    public void ChangeMasterVolume(float volume)
    {
        _masterVolume.Value = volume;
    }
    public void ChangeBGMVolume(float volume)
    {
        _bgmVolume.Value = volume;
    }
    public void ChangeSEVolume(float volume)
    {
        _seVolume.Value = volume;
    }

    public AudioManager()
    {
        MasterVolume.Subscribe(_ =>
        {
            _bgmPlayer.SetVolume(_masterVolume.Value * _bgmVolume.Value);
            _bgmPlayer.Update(_bgmPlayback);

            for (int i = 0; i < _seData.Count; i++)
            {
                if (_seData[i].IsLoop)
                {
                    _loopSEPlayer.SetVolume(_masterVolume.Value * _seVolume.Value);
                    _loopSEPlayer.Update(_seData[i].Playback);
                }
                else
                {
                    _sePlayer.SetVolume(_masterVolume.Value * _seVolume.Value);
                    _sePlayer.Update(_seData[i].Playback);
                }
            }
        });

        BGMVolume.Subscribe(_ =>
        {
            _bgmPlayer.SetVolume(_masterVolume.Value * _bgmVolume.Value);
            _bgmPlayer.Update(_bgmPlayback);
        });

        SEVolume.Subscribe(_ =>
        {
            for (int i = 0; i < _seData.Count; i++)
            {
                if (_seData[i].IsLoop)
                {
                    _loopSEPlayer.SetVolume(_masterVolume.Value * _seVolume.Value);
                    _loopSEPlayer.Update(_seData[i].Playback);
                }
                else
                {
                    _sePlayer.SetVolume(_masterVolume.Value * _seVolume.Value);
                    _sePlayer.Update(_seData[i].Playback);
                }
            }
        });

        SceneManager.sceneUnloaded += _ => StopLoopSE();
    }
    // ここに音を鳴らす関数を書いてください

    /// <summary>BGMを開始する</summary>
    /// <param name="cueSheetName">流したいキューシートの名前</param>
    /// <param name="cueName">流したいキューの名前</param>
    public void PlayBGM(string cueSheetName, string cueName)
    {
        var temp = CriAtom.GetCueSheet(cueSheetName).acb;

        if (_currentBGMAcb == temp && _currentBGMCueName == cueName &&
            _bgmPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            return;
        }

        StopBGM();

        _bgmPlayer.SetCue(temp, cueName);
        _bgmPlayback = _bgmPlayer.Start();
        _currentBGMAcb = temp;
        _currentBGMCueName = cueName;
    }

    public void PauseBGM()
    {
        if (_bgmPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            _bgmPlayer.Pause();
        }
    }

    public void ResumeBGM()
    {
        _bgmPlayer.Resume(CriAtomEx.ResumeMode.PausedPlayback);
    }

    public void StopBGM()
    {
        if (_bgmPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            _bgmPlayer.Stop();
        }
    }

    /// <summary>SEを流す関数</summary>
    /// <param name="cueSheetName">流したいキューシートの名前</param>
    /// <param name="cueName">流したいキューの名前</param>
    /// <returns>停止する際に必要なIndex</returns>
    public int PlaySE(string cueSheetName, string cueName, float volume = 1f)
    {
        CriAtomEx.CueInfo cueInfo;
        CriPlayerData newAtomPlayer = new CriPlayerData();
        
        var tempAcb = CriAtom.GetCueSheet(cueSheetName).acb;
        tempAcb.GetCueInfo(cueName, out cueInfo);

        newAtomPlayer.CueInfo = cueInfo;
        
        if (newAtomPlayer.IsLoop)
        {
            _loopSEPlayer.SetCue(tempAcb, cueName);
            _loopSEPlayer.SetVolume(volume * _seVolume.Value * _masterVolume.Value);
            newAtomPlayer.Playback = _loopSEPlayer.Start();
            
            Debug.Log($"new {volume * _seVolume.Value * _masterVolume.Value} {cueName} {_loopSEPlayer.GetStatus()}");
        }
        else
        {
            _sePlayer.SetCue(tempAcb, cueName);
            _sePlayer.SetVolume(volume * _seVolume.Value * _masterVolume.Value);
            newAtomPlayer.Playback = _sePlayer.Start();
            
            Debug.Log($"new {volume * _seVolume.Value * _masterVolume.Value} {cueName} {_sePlayer.GetStatus()}");
        }

        _seData.Add(newAtomPlayer);
        return _seData.Count - 1;
    }

    /// <summary>SEをPauseさせる </summary>
    /// <param name="index">一時停止させたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void PauseSE(int index)
    {
        if (index < 0) return;

        _seData[index].Playback.Pause();
    }

    /// <summary>PauseさせたSEを再開させる</summary>
    /// <param name="index">再開させたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void ResumeSE(int index)
    {
        if (index < 0) return; 

        _seData[index].Playback.Resume(CriAtomEx.ResumeMode.AllPlayback);
    }

    /// <summary>SEを停止させる </summary>
    /// <param name="index">止めたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void StopSE(int index)
    {
        if (index < 0) return;

        _seData[index].Playback.Stop();
    }

    /// <summary>ループしているすべてのSEを止める</summary>
    public void StopLoopSE()
    {
        _loopSEPlayer.Stop();
    }
}