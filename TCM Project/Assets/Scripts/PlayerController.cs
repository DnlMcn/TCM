using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: DnlMcn

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float movementSpeed; // Velocidade base do movimento
    public bool canRun; // Muda se o jogador é permitido correr
    public float sprintScale; // Valor que multiplica a velocidade base ao correr 
    public float gravity; // Força da gravidade
    public float jumpHeight; // Altura do pulo

    // Variáveis relacionando à estamina (em segundos);
    public float maxStamina;
    public float currentStamina;
    public float staminaRecoverSpeed;
    float jumpStaminaCost;
    bool isExhausted;
    bool isRested;

    bool isSprinting;// Verifica se o personagem está correndo

    public event System.Action OnItemPickup;
    public event System.Action OnSubtitleTrigger;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 velocity;
    Vector3 movement;
    bool isGrounded;

    private void Start()
    {
        currentStamina = maxStamina;
        jumpStaminaCost = maxStamina / (staminaRecoverSpeed * 3);
    }

    void Update()
    {
        // Pulo e gravidade

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Verifica se o jogador está no chão

        // Implementação simples de pulo
        if (Input.GetButtonDown("Jump") && isGrounded && !isExhausted)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentStamina -= jumpStaminaCost;
        }

        // Reduz a velocidade vertical do jogador se ele encostar no chão
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }


        // Movimento básico

        // Movimento básico com WASD
        if (isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            movement = transform.right * x + transform.forward * z;
        }
        controller.Move(movement * movementSpeed * Time.deltaTime);


        // Corrida e estamina

        if (canRun && isGrounded && !isExhausted && !isSprinting && Input.GetKey(KeyCode.LeftShift)) { Run(); }
        if (isGrounded && isExhausted && isSprinting || !Input.GetKey(KeyCode.LeftShift) && isSprinting) { StopRunning(); }

        if (isGrounded && !isSprinting && currentStamina < maxStamina) { RecoverStamina(); }
        if (isGrounded && isSprinting && currentStamina > 0) { ConsumeStamina(); }

        if (currentStamina <= 0) { isExhausted = true; Debug.Log("Você está exausto."); }
        if (currentStamina >= maxStamina && !isRested) { isRested = true; }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Run()
    {
        isSprinting = true;
        movementSpeed *= sprintScale;
    }

    void StopRunning()
    {
        isSprinting = false;
        movementSpeed /= sprintScale;
    }

    void ConsumeStamina()
    {
        currentStamina -= Time.deltaTime;
    }

    void RecoverStamina()
    {
        currentStamina += staminaRecoverSpeed * Time.deltaTime;
        if (currentStamina > maxStamina / 3 && isExhausted) { isExhausted = false; Debug.Log("Você não está mais exausto."); }
    }


    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.CompareTag("Item"))
        {
            Destroy(triggerCollider.gameObject);
            OnItemPickup?.Invoke();
        }
        if (triggerCollider.CompareTag("CabinPathBarrier"))
        {
            Debug.Log("Eu deveria seguir a trilha...");
        }
    }
}
