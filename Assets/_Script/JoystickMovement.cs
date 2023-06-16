using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class JoystickMovement : MonoBehaviour
{
    private MyJoystick myJoy;

    [SerializeField]
    FloatingJoystick fJoy;

    private CharacterController controller;
    public float characterSpeed;
    Animator animator;

    private float turnInput;
    private float moveInput;
    public TMP_Text pricetext;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool doOnceRun = false;
    bool doOnceIdle = true;
    public GameObject collectParent;
    int collectCount = 0;
    public List<Transform> collectedGems = new List<Transform>();
    float timeLeft = 2;
    float totalPrice ;

    int greenGemCount;
    int yellowGemCount;
    int pinkGemCount;
    public Image GemPanel;
    public TMP_Text greenGemText;
    public TMP_Text pinkGemText;
    public TMP_Text yellowGemText;

    private void Start()
    {
        myJoy = FindObjectOfType<MyJoystick>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        pricetext.text = PlayerPrefs.GetFloat("totalPrice").ToString("F0");
        yellowGemText.text = PlayerPrefs.GetInt("yellowGemCount").ToString("F0");
        pinkGemText.text = PlayerPrefs.GetInt("pinkGemCount").ToString("F0");
        greenGemText.text = PlayerPrefs.GetInt("greenGemCount").ToString("F0"); ;
        GemPanel.gameObject.SetActive(false);

    }
    void UpdateTotalPrice()
    {
        pricetext.text = totalPrice.ToString("F0");
        yellowGemText.text = yellowGemCount.ToString();
        pinkGemText.text = pinkGemCount.ToString();
        greenGemText.text = greenGemCount.ToString();
        PlayerPrefs.SetFloat("totalPrice", totalPrice);
        PlayerPrefs.SetInt("yellowGemCount", yellowGemCount);
        PlayerPrefs.SetInt("greenGemCount", greenGemCount);
        PlayerPrefs.SetInt("pinkGemCount", pinkGemCount);


    }

    public void openPanel()
    {
        GemPanel.gameObject.SetActive(true);
    }
    public void closePanel()
    {
        GemPanel.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {

        turnInput = fJoy.Horizontal;
        moveInput = fJoy.Vertical;


        if (myJoy.pressed == true)
        {
            Move();
            if (doOnceRun) { RunAnim(); doOnceIdle = true; doOnceRun = false; }

        }
        else
        {
            controller.Move((Vector3.zero));
            if (doOnceIdle) { IdleAnim(); doOnceRun = true; doOnceIdle = false; }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void Move()
    {
        Vector3 direction = new Vector3(turnInput, 0f, moveInput).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (characterSpeed * Time.deltaTime));

        }

    }
    public void GemText()
    {

    }
    public void RunAnim()
    {
        animator.SetBool("Run", true);
    }
    public void IdleAnim()
    {
        animator.SetBool("Run", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GemScript>())
        {

            Destroy(other.gameObject.GetComponent<Collider>());
            other.transform.DOKill();
            other.transform.SetParent(collectParent.transform);
            other.transform.DOLocalMove(Vector3.zero + new Vector3(0, 0.50f * collectCount, 0), 0.5f).SetEase(Ease.Flash);



            collectCount++;



            collectedGems.Add(other.transform);



        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                if (collectedGems.Count > 0)
                {

                    Transform tr = collectedGems[collectedGems.Count - 1];
                    tr.parent = other.transform;
                    tr.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
                    totalPrice += tr.gameObject.GetComponent<GemScript>().price + tr.transform.localScale.x * 100;
                    collectedGems.Remove(tr);
                    collectCount--;
                    
                    if (tr.gameObject.GetComponent<GemScript>().GemType == GemType.green)
                    {
                        greenGemCount++;
                    }
                    if (tr.gameObject.GetComponent<GemScript>().GemType == GemType.pink)
                    {
                        pinkGemCount++;
                    }
                    if (tr.gameObject.GetComponent<GemScript>().GemType == GemType.yellow)
                    {
                        yellowGemCount++;
                    }
                    UpdateTotalPrice();
                    timeLeft = 0.1f;
                }



            }
        }
    }


}
