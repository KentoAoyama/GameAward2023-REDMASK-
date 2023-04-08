// 日本語対応
using UniRx;

public class AudioManager
{
    private FloatReactiveProperty _masterVolume = new FloatReactiveProperty();
    private FloatReactiveProperty _bgmVolume = new FloatReactiveProperty();
    private FloatReactiveProperty _seVolume = new FloatReactiveProperty();

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

    // ここに音を鳴らす関数を書いてください
}