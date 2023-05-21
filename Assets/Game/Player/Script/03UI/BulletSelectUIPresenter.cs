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
        private BulletCountManager _bulletsManager = null;
        /// <summary> イメージのディクショナリ </summary>
        private Dictionary<BulletType, Image> _bulletIcons = new Dictionary<BulletType, Image>();
        /// <summary> テキストのディクショナリ </summary>
        private Dictionary<BulletType, Text> _bulletCountTexts = new Dictionary<BulletType, Text>();
        /// <summary> 現在選択している弾の種類 </summary>
        private ReactiveProperty<BulletType> _currentSelectBulletType = new ReactiveProperty<BulletType>(BulletType.NotSet);

        public IReadOnlyReactiveProperty<BulletType> CurrentSelectBulletType => _currentSelectBulletType;

        public void Init(InputManager inputManager, BulletCountManager bulletsManager)
        {
            _currentSelectBulletType.Value = BulletType.StandardBullet;
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
            _bulletsManager.StandardBulletCount.Subscribe(num => _standardBulletCountText.text = $"{num.ToString("00")}");
            _bulletsManager.PenetrateBulletCount.Subscribe(num => _penetrateBulletCountText.text = $"{num.ToString("00")}");
            _bulletsManager.ReflectBulletCount.Subscribe(num => _reflectBulletCountText.text = $"{num.ToString("00")}");

            _currentSelectBulletType.Subscribe(value =>
            {
                foreach (var e in _bulletCountTexts) e.Value.enabled = false;
                _bulletCountTexts[value].enabled = true;
                foreach (var e in _bulletIcons) e.Value.enabled = false;
                _bulletIcons[value].enabled = true;
            });
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
                var a = _currentSelectBulletType.Value;
                a++;
                a = a >= BulletType.ShellCase ? BulletType.NotSet + 1 : a;
                _currentSelectBulletType.Value = a;
                // 加算後の値を範囲内に収める処理。（範囲外になった場合、BulletType.NotSetの次の値を代入する。）
                return _currentSelectBulletType.Value;

            }
            else // 左入力が発生したとき
            {
                var a = _currentSelectBulletType.Value;
                a--;
                a = a > BulletType.NotSet ? a : BulletType.ShellCase - 1;
                _currentSelectBulletType.Value = a;
                return _currentSelectBulletType.Value;
            }
        }
        /// <summary> 矢印のx座標を,選択している弾のアイコンのx座標と同じにする。 </summary>
        private void UpdateArrayPos(BulletType type)
        {
            //var pos = _currentSelectedArrowImage.transform.position;
            //pos.x = _bulletIcons[type].transform.position.x;
            //_currentSelectedArrowImage.transform.position = pos;
        }
    }
}