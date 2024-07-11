using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HungryWorm
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private UIScreen m_SplashScreen;
        [SerializeField] private UIScreen m_StartScreen;
        [SerializeField] private UIScreen m_MainMenuScreen;
        [SerializeField] private UIScreen m_SettingsScreen;
        [SerializeField] private UIScreen m_GameScreen;
        [SerializeField] private UIScreen m_PauseScreen;
        [SerializeField] private UIScreen m_GameSettingsScreen;
        [SerializeField] private UIScreen m_EndScreen; 
        [SerializeField] private UIScreen m_LeaderboardScreen;
        
        // The currently active UIScreen
        private UIScreen m_CurrentScreen;
        
        // A stack of previously displayed UIScreens
        Stack<UIScreen> m_History = new Stack<UIScreen>();

        // A list of all Views to show/hide
        List<UIScreen> m_Screens = new List<UIScreen>();
        
        public UIScreen CurrentScreen => m_CurrentScreen;
        
        // Register event listeners to game events
        private void OnEnable()
        {
            SubscribeToEvents();

            // Because non-MonoBehaviours can't run coroutines, the Coroutines helper utility allows us to
            // designate a MonoBehaviour to manage starting/stopping coroutines
            Coroutines.Initialize(this);

            Initialize();
        }
        
        // Unregister the listeners to prevent errors
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            // Wait for the SplashScreen to finish loading then load the StartScreen
            SceneEvents.PreloadCompleted += SceneEvents_PreloadCompleted;

            // Pair GameEvents with methods to Show each screen
            UIEvents.SplashScreenShown += UIEvents_SplashScreenShown;
            UIEvents.MainMenuShown += UIEvents_MainMenuShown;
            UIEvents.SettingsShown += UIEvents_SettingsShown;
            UIEvents.GameScreenShown += UIEvents_GameScreenShown;
            UIEvents.PauseScreenShown += UIEvents_PauseScreenShown;
            UIEvents.EndScreenShown += UIEvents_EndScreenShown;
            UIEvents.LeaderboardScreenShown += UIEvents_LeaderboardScreenShown;
            UIEvents.ScreenClosed += UIEvents_ScreenClosed;
        }
        
        private void UnsubscribeFromEvents()
        {
            SceneEvents.PreloadCompleted -= SceneEvents_PreloadCompleted;

            UIEvents.SplashScreenShown -= UIEvents_SplashScreenShown;
            UIEvents.MainMenuShown -= UIEvents_MainMenuShown;
            UIEvents.SettingsShown -= UIEvents_SettingsShown;
            UIEvents.GameScreenShown -= UIEvents_GameScreenShown;
            UIEvents.PauseScreenShown -= UIEvents_PauseScreenShown;
            UIEvents.EndScreenShown -= UIEvents_EndScreenShown;
            UIEvents.LeaderboardScreenShown -= UIEvents_LeaderboardScreenShown;
            UIEvents.ScreenClosed -= UIEvents_ScreenClosed;
        }
        
        #region Event-handling methods

        // Show the SplashScreen and don't keep in history
        private void UIEvents_SplashScreenShown()
        {
            Show(m_SplashScreen, false);
        }

        // Show the StartScreen but don't keep it in the history
        private void SceneEvents_PreloadCompleted()
        {
            Show(m_StartScreen, false);
        }
        
        private void UIEvents_MainMenuShown()
        {
            m_CurrentScreen = m_MainMenuScreen;

            HideScreens();
            m_History.Push(m_MainMenuScreen);
            m_MainMenuScreen.Show();
        }
        
        private void UIEvents_SettingsShown()
        {
            Show(m_SettingsScreen);
        }
        
        private void UIEvents_GameScreenShown()
        {
            Show(m_GameScreen);
        }
        private void UIEvents_PauseScreenShown()
        {
            Show(m_PauseScreen);
        }

        private void UIEvents_EndScreenShown()
        {
            Show(m_EndScreen);
        }
        
        private void UIEvents_LeaderboardScreenShown()
        {
            Show(m_LeaderboardScreen);
        }
        
        // Remove the top UI screen from the stack and make that active (i.e., go back one screen)
        public void UIEvents_ScreenClosed()
        {
            if (m_History.Count != 0)
            {
                Show(m_History.Pop(), false);
            }
        }
        
        
        #endregion
        
        private void Initialize()
        {
            NullRefChecker.Validate(this);
            
            RegisterScreens();
            InitializeScreens();
            HideScreens();
        }
        
        // Store each UIScreen into a master list so we can hide all of them easily.
        private void RegisterScreens()
        {
            m_Screens = new List<UIScreen>
            {
                m_SplashScreen,
                m_StartScreen,
                m_MainMenuScreen,
                m_SettingsScreen,
                m_GameScreen,
                m_PauseScreen,
                m_GameSettingsScreen,
                m_EndScreen,
                m_LeaderboardScreen
            };
        }
        
        // Initialize each UIScreen
        private void InitializeScreens()
        {
            foreach (UIScreen screen in m_Screens)
            {
                screen.Initialize();
            }
        }
        
        // Clear history and hide all Views
        private void HideScreens()
        {
            m_History.Clear();

            foreach (UIScreen screen in m_Screens)
            {
                screen.Hide();
            }
        }
        
        public void Show(UIScreen screen, bool keepInHistory = true)
        {
            if (screen == null)
                return;

            if (m_CurrentScreen != null)
            {
                if (!screen.IsTransparent)
                    m_CurrentScreen.Hide();

                if (keepInHistory)
                {
                    m_History.Push(m_CurrentScreen);
                }
            }

            screen.Show();
            m_CurrentScreen = screen;
        }
        
        // Shows a UIScreen with the keepInHistory always enabled
        public void Show(UIScreen screen)
        {
            Show(screen, true);
        }


    }
}