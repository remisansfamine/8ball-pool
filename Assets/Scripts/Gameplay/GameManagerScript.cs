using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private List<PlayerBehavior> players;
    private PlayerBehavior currentPlayer;
    private int currentPlayerID = 0;

    [SerializeField] private Ball mainBall;
    [SerializeField] private Vector2 pos = Vector2.zero;

    [SerializeField] private TMP_Text currPlayer;
    [SerializeField] private TMP_Text scorePlayer_1;
    [SerializeField] private TMP_Text scorePlayer_2;


    bool turnLaunched = true;

    [SerializeField] private float maxSpeed = 1f;

    [SerializeField] private float epsilonVelocity = 0.001f;

    Vector3 pushOrigin;

    [Header("Slider")]
    [SerializeField] private Slider slider;
    [SerializeField] private float sliderCoef = 0.05f;
    private float sliderForceCoef = 0.05f;
    bool sliderUp = false;
    bool sliderLock = false;

    [Header("PoolCue")]
    [SerializeField] private GameObject poolCue;
    [SerializeField] private GameObject sprite;
    [SerializeField] private float force = 1f;
    [SerializeField] private float angleForce = 1f;

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(SliderEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = players[currentPlayerID];
    }

    // Update is called once per frame
    void Update()
    {
        if (turnLaunched)
        {
            poolCue.gameObject.SetActive(true);
            Play();
        }
        else if (CheckTurnPassed())
        {
            ChangeTurn();
        }

        UpdateUI();

    }

    void UpdateUI()
    {
        currPlayer.text = "Joueur " + (currentPlayerID + 1).ToString();
        scorePlayer_1.text = "Joueur 1 = " + players[0].score.ToString();
        scorePlayer_2.text = "Joueur 2 = " + players[1].score.ToString();
    }

    void ChangeTurn()
    {
        currentPlayerID = (currentPlayerID + 1) % players.Count;

        currentPlayer = players[currentPlayerID];

        turnLaunched = true;
    }

    private IEnumerator sliderCoroutine()
    {
        sliderLock = true;
        while (slider.value != 1)
        {
            slider.value += 1 * sliderForceCoef;

            yield return null;
        }

        sliderLock = false;
        slider.enabled = true;

    }


    public void SliderEvent(float value)
    {
        if (!sliderUp && !sliderLock)
            sliderUp = true;
    }

    private void Play()
    {
        poolCue.transform.position = mainBall.transform.position;


        if (Input.GetButtonUp("Shoot") && sliderUp)
        {
            if (slider.value != 1.0f)
            {
                Debug.Log("Force = " + force * (1 - slider.value));
                slider.enabled = false;

                sliderForceCoef = (1 - slider.value) * sliderCoef;

                StartCoroutine("sliderCoroutine");

                Vector3 direction = -poolCue.transform.forward;
                Vector3 forceVec = new Vector3(direction.x * force, 0f, direction.z * force);

                //Vector3 pos = new Vector3(0.0f, 0f, 0f);
                Vector3 currforce = Vector3.ClampMagnitude(forceVec, maxSpeed);
                mainBall.AddLocalTorque(currforce, pos);
                mainBall.AddForce(currforce);
                //mainBall.velocity = Vector3.ClampMagnitude(forceVec, maxSpeed);

                poolCue.gameObject.SetActive(false);

            }

            turnLaunched = sliderUp = false;
        }
        
    }
    bool CheckTurnPassed() => mainBall.Velocity.sqrMagnitude < epsilonVelocity;

    public void HoleCallback(Ball puttedBall)
    {
        if (puttedBall != mainBall)
            currentPlayer.score++;
        else
            Application.Quit();
    }

    //private void OnGUI()
    //{
    //    GUIContent content = new GUIContent();
    //    for (int i = 0; i < players.Count; i++)
    //        content.text += "Player " + (i + 1) + " : " + players[i].score + '\n';
    //
    //
    //    content.text += "Current player: " + (currentPlayerID + 1) + '\n';
    //
    //    GUI.Label(new Rect(10, 10, 150, 100), content);
    //}
}
