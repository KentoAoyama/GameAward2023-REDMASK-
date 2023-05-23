// 日本語対応
using System.Collections.Generic;
using UniRx;
using CriWare;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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
    private List<CriPlayerData> _sePlayerData = new List<CriPlayerData>();
    private string _currentBGMCueName = "";
    private CriAtomExAcb _currentBGMAcb = null;

    public IReadOnlyReactiveProperty<float> MasterVolume => _masterVolume;
    public IReadOnlyReactiveProperty<float> BGMVolume => _bgmVolume;
    public IReadOnlyReactiveProperty<float> SEVolume => _seVolume;

    private const string SaveFileName = "AudioVolume";

    struct CriPlayerData
    {
        private CriAtomExPlayer _player;
        private CriAtomExPlayback _playback;
        private CriAtomEx.CueInfo _cueInfo;

        public CriAtomExPlayer Player
        {
            get => _player;
            set => _player = value;
        }

        public CriAtomExPlayback Playback => _playback;
        public CriAtomEx.CueInfo CueInfo => _cueInfo;

        public void Start()
        {
            _playback = _player.Start();
        }

        public void SetCue(CriAtomExAcb acb, string cueName)
        {
            _player.SetCue(acb, cueName);
            acb.GetCueInfo(cueName, out _cueInfo);
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

            for (int i = 0; i < _sePlayerData.Count; i++)
            {
                _sePlayerData[i].Player.SetVolume(_masterVolume.Value * _seVolume.Value);
                _sePlayerData[i].Player.Update(_sePlayerData[i].Playback);
            }
        });

        BGMVolume.Subscribe(_ =>
        {
            _bgmPlayer.SetVolume(_masterVolume.Value * _bgmVolume.Value);
            _bgmPlayer.Update(_bgmPlayback);
        });

        SEVolume.Subscribe(_ =>
        {
            for (int i = 0; i < _sePlayerData.Count; i++)
            {
                _sePlayerData[i].Player.SetVolume(_masterVolume.Value * _seVolume.Value);
                _sePlayerData[i].Player.Update(_sePlayerData[i].Playback);
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
        for (int i = 0; i < _sePlayerData.Count; i++)
        {
            // PlayerのStatusがPrepかPlayingじゃないときにならす
            if (!(_sePlayerData[i].Player.GetStatus() == CriAtomExPlayer.Status.Playing || _sePlayerData[i].Player.GetStatus() == CriAtomExPlayer.Status.Prep))
            {
                var temp = CriAtom.GetCueSheet(cueSheetName).acb;

                _sePlayerData[i].Player.SetVolume(volume * _seVolume.Value * _masterVolume.Value);
                _sePlayerData[i].SetCue(temp, cueName);
                _sePlayerData[i].Start();

                if (_sePlayerData[i].Player.GetStatus() == CriAtomExPlayer.Status.Error)
                {
                    _sePlayerData[i].Player.Stop();
                    _sePlayerData[i].Player.Dispose();
                    _sePlayerData.Remove(_sePlayerData[i]);

                    continue;
                }

                return i;
            }
        }

        CriPlayerData newAtomPlayer  = default;
        newAtomPlayer.Player = new CriAtomExPlayer();

        var tempAcb = CriAtom.GetCueSheet(cueSheetName).acb;

        newAtomPlayer.Player.SetVolume(volume * _seVolume.Value * _masterVolume.Value);
        newAtomPlayer.SetCue(tempAcb, cueName);
        newAtomPlayer.Start();

        _sePlayerData.Add(newAtomPlayer);
        return _sePlayerData.Count - 1;
    }

    /// <summary>SEをPauseさせる </summary>
    /// <param name="index">一時停止させたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void PauseSE(int index)
    {
        if (index < 0) return;

        if (_sePlayerData[index].Player.GetStatus() == CriAtomExPlayer.Status.Playing || _sePlayerData[index].Player.GetStatus() == CriAtomExPlayer.Status.Prep)
        {
            _sePlayerData[index].Player.Pause();
        }
    }

    /// <summary>PauseさせたSEを再開させる</summary>
    /// <param name="index">再開させたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void ResumeSE(int index)
    {
        if (index < 0) return; 

        _sePlayerData[index].Player.Resume(CriAtomEx.ResumeMode.AllPlayback);
    }

    /// <summary>SEを停止させる </summary>
    /// <param name="index">止めたいPlaySE()の戻り値 (-1以下を渡すと処理を行わない)</param>
    public void StopSE(int index)
    {
        if (index < 0) return;

        if (_sePlayerData[index].Player.GetStatus() == CriAtomExPlayer.Status.Playing || _sePlayerData[index].Player.GetStatus() == CriAtomExPlayer.Status.Prep)
        {
            _sePlayerData[index].Player.Stop();
        }
    }

    /// <summary>ループしているすべてのSEを止める</summary>
    public void StopLoopSE()
    {
        for (int i = 0; i < _sePlayerData.Count; i++)
        {
            if (_sePlayerData[i].CueInfo.length <= -1)
            {
                _sePlayerData[i].Player.Stop();
            }
        }
    }
}