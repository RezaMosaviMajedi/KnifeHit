using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int level=0;
    int score = 0;
    int apples = 0;


    public float throwingSpeed=5;

    [SerializeField]
    DataLevels dataLevels;

    [SerializeField]
    TrunkController trunk;

    public GameObject knivePrefab;
    GameObject currentKnive;
    List<GameObject> knivesPooling = new List<GameObject>();

    public GameObject applePrefab;

    [Header("UI")]
    public GameObject adsMenu;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtApples, txtKnives;

    private void Awake()
    {
        Instance = this;
    }
    public void PlayGame()
    {
        trunk.SetCurrntLevel(dataLevels.dataLevels[level]);
        score = 0;
        txtLevel.text = $"Level {level + 1}";
        txtApples.text = apples.ToString();
        txtKnives.text = dataLevels.dataLevels[level].maxScore.ToString();
        SetKnives();
    }

    public void AddAppel()
    {
        apples++;
        txtApples.text = apples.ToString();
    }

    public void AddScore()
    {
        score++;
        txtKnives.text = (dataLevels.dataLevels[level].maxScore-score).ToString();

        if (score == dataLevels.dataLevels[level].maxScore)
        {
            trunk.DestroyTrunk();
            Invoke("NextLevel", 2);
        }
        else
        {
            SetKnives();
        }
    }
    void NextLevel()
    {
        foreach (var item in knivesPooling)
        {
            item.SetActive(false);
        }

        level++;
        if(level==dataLevels.dataLevels.Length)
            level = 0;

        PlayGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentKnive == null)
                return;

            currentKnive.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            currentKnive.GetComponent<Rigidbody2D>().velocity = Vector2.up * throwingSpeed;
            currentKnive.GetComponent<Collider2D>().isTrigger = false;

            currentKnive = null;
        }
    }

    public void SetKnives()
    {
        currentKnive = GetKnive();
        currentKnive.SetActive(true);
        currentKnive.GetComponent<Collider2D>().isTrigger = true;
        currentKnive.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    GameObject GetKnive()
    {
        GameObject knive = null;
        if (knivesPooling.Count > 0)
        {
            for (int i = 0; i < knivesPooling.Count; i++)
            {
                if (knivesPooling[i].activeSelf == false)
                {
                    knive = knivesPooling[i];
                    break;
                }
            }
            if (knive != null)
            {
                knive.transform.position = transform.position;
                knive.transform.rotation = Quaternion.identity;
                return knive;
            }
        }

        knive = Instantiate(knivePrefab, transform.position, Quaternion.identity);
        knivesPooling.Add(knive);

        return knive;
    }

    public void RestartGame()
    {
        if (adsMenu.activeSelf)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Time.timeScale = 0;
            adsMenu.SetActive(true);
        }
    }
    public void LoadAdsResume()
    {
        GetComponent<GoogleAdmobManager>().LoadRewardedAd(0, "Resume");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        adsMenu.SetActive(false);

        Invoke("SetKnives",1);
    }
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}