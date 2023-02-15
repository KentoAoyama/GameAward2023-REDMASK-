// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace GameSystem
{
    public class GameModeManager
    {
        #region Singleton
        private static GameModeManager _instance = new GameModeManager();
        public static GameModeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"Error! Please correct!");
                }
                return _instance;
            }
        }
        private GameModeManager() { }
        #endregion

        private ReactiveProperty<GameMode> _currentgameMode = new ReactiveProperty<GameMode>();

        public IReadOnlyReactiveProperty<GameMode> CurrentGameMode => _currentgameMode;

        private void Setup()
        {
            _currentgameMode.Value = GameMode.NotSet;
        }
    }

    public enum GameMode
    {
        NotSet,
        Title,
    }
}