using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private List<PlayerBehavior> players;
    private PlayerBehavior currentPlayer;
    private int currentPlayerID = 0;

    [SerializeField] private MomentumBehavior mainBall;

    bool turnLaunched = true;

    [SerializeField] private float force = 1f;
    [SerializeField] private float maxSpeed = 1f;

    [SerializeField] private float epsilonVelocity = 0.001f;

    Vector3 pushOrigin;

    bool canClickUp = false;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = players[currentPlayerID];
    }

    // Update is called once per frame
    void Update()
    {
        if (turnLaunched)
            Play();
        else if (CheckTurnPassed())
            ChangeTurn();
    }

    void ChangeTurn()
    {
        currentPlayerID = (currentPlayerID + 1) % players.Count;

        currentPlayer = players[currentPlayerID];

        turnLaunched = true;
    }

    private void Play()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
                pushOrigin = hit.point;

            canClickUp = true;
        }

        if (Input.GetButtonUp("Shoot") && canClickUp)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            Vector3 direction = pushOrigin - hit.point;
            Vector3 forceVec = new Vector3(direction.x * force, 0f, direction.z * force);

            mainBall.velocity = Vector3.ClampMagnitude(forceVec, maxSpeed);

            turnLaunched = canClickUp = false;
        }
    }
    bool CheckTurnPassed() => mainBall.velocity.sqrMagnitude < epsilonVelocity;

    public void HoleCallback(MomentumBehavior puttedBall)
    {
        if (puttedBall != mainBall)
            currentPlayer.score++;
        else
            Application.Quit();
    }

    private void OnGUI()
    {
        GUIContent content = new GUIContent();
        for (int i = 0; i < players.Count; i++)
            content.text += "Player " + (i + 1) + " : " + players[i].score + '\n';


        content.text += "Current player: " + (currentPlayerID + 1) + '\n';

        GUI.Label(new Rect(10, 10, 150, 100), content);
    }
}
