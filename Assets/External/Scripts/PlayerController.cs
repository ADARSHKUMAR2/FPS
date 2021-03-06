using System;
using System.Collections;
using System.Collections.Generic;
using External.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks,IDamageable
{
    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject uiHUD;
    
    [SerializeField] private Item[] items;
    [SerializeField] private Image healthBarImage;
    
    private float verticalLookRotation;
    private bool isGrounded;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;

    private Rigidbody rb;
    private PhotonView PV;

    private int itemIndex;
    private int prevItemIndex = -1;
    
    const float maxHealth = 100f;
    private float currHealth = maxHealth;

    private PlayerManager playerManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);   
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(uiHUD);
        }
    }

    private void Update()
    {
        if(!PV.IsMine)
            return;
        Look();
        Move();
        Jump();
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            if (itemIndex >= items.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            if (itemIndex <= 0)
                EquipItem(items.Length - 1);
            else
                EquipItem(itemIndex - 1);

        if (Input.GetMouseButtonDown(0))
            items[itemIndex].Use();
        
        if(transform.position.y < -10f)
            Die();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(transform.up * jumpForce);
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount,
            moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void SetGroundedState(bool _grounded)
    {
        isGrounded = _grounded;
    }

    private void EquipItem(int _index)
    {
        if(_index == prevItemIndex)
            return;
        
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        
        if(prevItemIndex!=-1)
            items[prevItemIndex].itemGameObject.SetActive(false);

        prevItemIndex = itemIndex;

        //This is to send our data to other clients
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex",itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    /// <summary>
    /// This is to receive changed properties of other players
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    private void FixedUpdate()
    {
        if(!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        PV.RPC("RPC_TakeDamage",RpcTarget.All,damageAmount);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
            return;

        healthBarImage.fillAmount = currHealth / maxHealth;
        
        Debug.Log($"Took Damage {damage}");
        currHealth -= damage;
        if (currHealth < 0f)
            Die();
    }

    private void Die()
    {
        playerManager.Die();
    }
}
