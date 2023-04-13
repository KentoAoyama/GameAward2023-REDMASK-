// 日本語対応
using System.Collections.Generic;
using UniRx;
using CriWare;

public class AudioManager
{
    private FloatReactiveProperty _masterVolume = new FloatReactiveProperty(1f);
    private FloatReactiveProperty _bgmVolume = new FloatReactiveProperty(1f);
    private FloatReactiveProperty _seVolume = new FloatReactiveProperty(1f);

    private CriAtomExPlayer _bgmPlayer = new CriAtomExPlayer();
    private CriAtomExPlayback _bgmPlayback;
    private List<CriAtomExPlayer> _sePlayer = new List<CriAtomExPlayer>();
    private List<CriAtomExPlayback> _sePlayback = new List<CriAtomExPlayback>();

    public IReadOnlyReactiveProperty<float> MasterVolume => _masterVolume;
    public IReadOnlyReactiveProperty<float> BGMVolume => _bgmVolume;
    public IReadOnlyReactiveProperty<float> SEVolume => _seVolume;

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

            for (int i = 0; i < _sePlayer.Count; i++)
            {
                _sePlayer[i].SetVolume(_masterVolume.Value * _seVolume.Value);
                _sePlayer[i].Update(_sePlayback[i]);
            }
        });

        BGMVolume.Subscribe(_ => 
        {
            _bgmPlayer.SetVolume(_masterVolume.Value * _bgmVolume.Value);
            _bgmPlayer.Update(_bgmPlayback);
        });

        SEVolume.Subscribe(_ =>
        {
            for(int i = 0; i < _sePlayer.Count; i++)
            {
                _sePlayer[i].SetVolume(_masterVolume.Value * _seVolume.Value);
                _sePlayer[i].Update(_sePlayback[i]);
            }
        });

        CriAtom.AddCueSheet("CueSheet_0", "CueSheet_0.acb", "CueSheet_0.awb");
    }
    // ここに音を鳴らす関数を書いてください

    /// <summary>BGMを開始する</summary>
    /// <param name="cueSheetName">流したいキューシートの名前</param>
    /// <param name="cueName">流したいキューの名前</param>
    public void PlayBGM(string cueSheetName, string cueName)
    {
        StopBGM();

        var temp = CriAtom.GetCueSheet(cueSheetName).acb;

        _bgmPlayer.SetCue(temp, cueName);
        _bgmPlayback = _bgmPlayer.Start();
    }

    public void StopBGM()
    {
        if (_bgmPlayer.GetStatus() != CriAtomExPlayer.Status.Playing)
        {
            return;
        }

        _bgmPlayer.Stop();
    }

    /// <summary>SEを流す関数</summary>
    /// <param name="cueSheetName">流したいキューシートの名前</param>
    /// <param name="cueName">流したいキューの名前</param>
    /// <returns>停止する際に必要なIndex</returns>
    public int PlaySE(string cueSheetName, string cueName, float volume = 1f)
    {


        for(int i = 0; i < _sePlayer.Count; i++)
        {
            if (_sePlayer[i].GetStatus() != CriAtomExPlayer.Status.Playing)
            {
                var temp = CriAtom.GetCueSheet(cueSheetName).acb;

                _sePlayer[i].SetVolume(volume * _seVolume.Value * _masterVolume.Value);
                _sePlayer[i].SetCue(temp, cueName);
                _sePlayback[i] = _sePlayer[i].Start();

                if (_sePlayer[i].GetStatus() == CriAtomExPlayer.Status.Error)
                {
                    _sePlayer[i].Dispose();
                    _sePlayer.Remove(_sePlayer[i]);

                    continue;
                }

                return i;
            }
        }

        var newAtomPlayer = new CriAtomExPlayer();

        var tempAcb = CriAtom.GetCueSheet(cueSheetName).acb;

        newAtomPlayer.SetVolume(volume * _seVolume.Value * _masterVolume.Value);
        newAtomPlayer.SetCue(tempAcb, cueName);
        _sePlayback.Add(newAtomPlayer.Start());

        _sePlayer.Add(newAtomPlayer);
        return _sePlayer.Count - 1;
    }

    /// <summary>SEを停止させる</summary>
    /// <param name="index">止めたいPlaySE()の戻り値</param>
    public void StopSE(int index)
    {
        if (_sePlayer[index].GetStatus() != CriAtomExPlayer.Status.Playing)
        {
            return ;
        }

        _sePlayer[index].Stop();
    }
}