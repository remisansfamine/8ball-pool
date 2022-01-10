using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointCounterScript : MonoBehaviour
{
    [SerializeField] private List<PlayerBehavior> players;

    private PlayerBehavior currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoint()
    {
        currentPlayer.score++;
    }

    private void OnGUI()
    {
        GUIContent content = new GUIContent();
        for (int i = 0; i < players.Count; i++)
            content.text += "Player " + i + " : " + players[i].score + '\n';

        GUI.Label(new Rect(10, 10, 150, 100), content);
    }
}
