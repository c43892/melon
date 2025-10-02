using UnityEngine;
using Framework;

public class GameDriver : MonoBehaviour
{
    public GameCore GameCore { get; set; }

    private void Start()
    {
        GameCore = new GameCore();
        GameCore.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        GameCore.OnTimeElapsed(Time.deltaTime);
    }
}
