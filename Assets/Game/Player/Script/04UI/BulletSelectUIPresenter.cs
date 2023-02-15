using Bullet;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI
{
    /// <summary>
    /// "弾の数、現在選択している弾"のビューへのプレゼンター
    /// </summary>
    [System.Serializable]
    public class BulletSelectUIPresenter
    {
        [Header("Images")]
        [Tooltip("標準的な弾を表現するアイコン"), SerializeField]
        private Image _standardBulletCountIcon = default;
        [Tooltip("貫通する弾を表現するアイコン"), SerializeField]
        private Image _penetrateBulletCountIcon = default;
        [Tooltip("反射する弾を表現するアイコン"), SerializeField]
        private Image _reflectBulletCountIcon = default;
        [Tooltip("現在選択している弾を表現するための矢印スプライト"), SerializeField]
        private Image _currentSelectedArrowImage = default;

        [Header("Texts")]
        [Tooltip("標準的な弾の所持数を描画するテキスト"), SerializeField]
        private Text _standardBulletCountText = default;
        [Tooltip("貫通する弾の所持数を描画するテキスト"), SerializeField]
        private Text _penetrateBulletCountText = default;
        [Tooltip("反射する弾の所持数を描画するテキスト"), SerializeField]
        private Text _reflectBulletCountText = default;

        /// <summary> 入力管理クラス </summary>
        private InputManager _inputManager = null;
        /// <summary> 弾数を管理するクラス </summary>
        private BulletsManager _bulletsManager = null;
        /// <summary> イメージのディクショナリ </summary>
        private Dictionary<BulletType, Image> _bulletIcons = new Dictionary<BulletType, Image>();
        /// <summary> テキストのディクショナリ </summary>
        private Dictionary<BulletType, Text> _bulletCountTexts = new Dictionary<BulletType, Text>();
        /// <summary> 現在選択している弾の種類 </summary>
        private BulletType _currentSelectBulletType = BulletType.NotSet;

        public void Init(InputManager inputManager, BulletsManager bulletsManager)
        {
            _currentSelectBulletType = BulletType.StandardBullet;
            _inputManager = inputManager;
            _bulletsManager = bulletsManager;
            // ディクショナリのセットアップ
            // イメージ
            _bulletIcons.Add(BulletType.StandardBullet, _standardBulletCountIcon);
            _bulletIcons.Add(BulletType.PenetrateBullet, _penetrateBulletCountIcon);
            _bulletIcons.Add(BulletType.ReflectBullet, _reflectBulletCountIcon);
            // テキスト
            _bulletCountTexts.Add(BulletType.StandardBullet, _standardBulletCountText);
            _bulletCountTexts.Add(BulletType.PenetrateBullet, _penetrateBulletCountText);
            _bulletCountTexts.Add(BulletType.ReflectBullet, _reflectBulletCountText);
            // いろいろ変化したときに実行するメソッドをサブスクライブする。
            //_bulletsManager.StandardBulletCount.Subscribe(num => _standardBulletCountText.text = $"標準の弾の数 :{num}");
            //_bulletsManager.PenetrateBulletCount.Subscribe(num => _penetrateBulletCountText.text = $"貫通弾の弾の数 :{num}");
            //_bulletsManager.ReflectBulletCount.Subscribe(num => _reflectBulletCountText.text = $"反射弾の弾の数 :{num}");
        }
        public void Update()
        {
            // 横入力が発生したときに矢印の位置を変更する
            if (_inputManager.IsPressed[InputType.HorizontalSelect])
            {
                UpdateArrayPos(OnInput(_inputManager.GetValue<float>(InputType.HorizontalSelect)));
            }
        }
        /// <summary> 入力が発生したときの処理 </summary>
        /// <param name="inputValue"> 入力値 </param>
        /// <returns> 変更後の値 </returns>
        private BulletType OnInput(float inputValue)
        {
            if (inputValue > 0) // 右入力が発生したとき
            {
                _currentSelectBulletType++;
                // 加算後の値を範囲内に収める処理。（範囲外になった場合、BulletType.NotSetの次の値を代入する。）
                _currentSelectBulletType = _currentSelectBulletType >= BulletType.End ? BulletType.NotSet + 1 : _currentSelectBulletType;
                return _currentSelectBulletType;

            }
            else // 左入力が発生したとき
            {
                _currentSelectBulletType--;
                // 減産後の値を範囲内に収める処理。（範囲外になった場合、BulletType.Endの一つ前の値を代入する。）
                _currentSelectBulletType = _currentSelectBulletType > BulletType.NotSet ? _currentSelectBulletType : BulletType.End - 1;
                return _currentSelectBulletType;
            }
        }
        /// <summary> 矢印のx座標を,選択している弾のアイコンのx座標と同じにする。 </summary>
        private void UpdateArrayPos(BulletType type)
        {
            var pos = _currentSelectedArrowImage.transform.position;
            pos.x = _bulletIcons[type].transform.position.x;
            _currentSelectedArrowImage.transform.position = pos;
        }
    }
}