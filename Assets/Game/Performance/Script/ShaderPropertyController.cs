public class ShaderPropertyController
{
    /// <summary>モノクロを管理するクラス</summary>
    private MonochromeController _monochromeController = new MonochromeController();
    /// <summary>写真風のエフェクトを管理するクラス</summary>
    private PictureController _pictureController = new PictureController();

    /// <summary>モノクロを管理するクラス</summary>
    public MonochromeController MonochromeController
    {
        get => _monochromeController;
    }

    /// <summary>写真風のエフェクトを管理するクラス</summary>
    public PictureController PictureController
    {
        get => _pictureController;
    }
}
