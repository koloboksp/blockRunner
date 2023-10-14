using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIStartPanel _startPanel;
    [SerializeField] private UIRestartPanel _restartPanel;
    [SerializeField] private UIGamePanel _gamePanel;

    public UIStartPanel StartPanel => _startPanel;
    public UIRestartPanel RestartPanel => _restartPanel;
    public UIGamePanel GamePanel => _gamePanel;
}