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

        public ReadOnlyReactiveProperty<GameMode> CurrentGameMode = null;

        private void Setup()
        {
            CurrentGameMode = new ReadOnlyReactiveProperty<GameMode>(_currentgameMode);
            _currentgameMode.Value = GameMode.NotSet;
        }
    }

    public enum GameMode
    {
        NotSet,
        Title,
    }
}