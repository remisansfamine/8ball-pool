using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player
{
    public int score;
}

public class PointCounterScript : MonoBehaviour
{
    private Player[] players;

    private Player currentPlayer;

    [SerializeField] int playerCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        players = new Player[playerCount];

        for (int i = 0; i < playerCount; i++)
            players[i] = new Player();

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
        for (int i = 0; i < players.Length; i++)
            content.text += "Player " + i + " : " + players[i].score + '\n';

        GUI.Label(new Rect(10, 10, 150, 100), content);
    }
}
