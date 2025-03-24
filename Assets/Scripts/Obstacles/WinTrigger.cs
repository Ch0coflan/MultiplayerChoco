using System;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class WinManager : MonoBehaviour
    {
        private bool _isGameWinned = false;
   
    private void OnEnable()
        {
            EventManager.OnPlayerWin += WinRequirementsCompleted;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerWin -= WinRequirementsCompleted;
        }

        private void WinRequirementsCompleted()
        {
            _isGameWinned = true;
            if (_isGameWinned)
            {
                Time.timeScale = 0;
            }
        }
    }
}
