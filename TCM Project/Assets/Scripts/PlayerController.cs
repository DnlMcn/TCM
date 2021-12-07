using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: DnlMcn

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float movementSpeed; // Velocidade base do movimento
    public float sprintScale; // Valor que multiplica a velocidade base ao correr 
    public float gravity; // Força da gravidade
    public float jumpHeight; // Altura do pulo 

    // Variáveis relacionando à estamina (em segundos);
    public float maxStamina; 
    public float currentStamina;
    public float staminaRecoverSpeed;
    bool isExhausted;                
    bool isRested; 
   
    bool isSprinting;// Verifica se o personagem está correndo

    public event System.Action OnItemPickup;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector3 movement;
    bool isGrounded;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Pulo e gravidade

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Verifica se o jogador está no chão

        // Implementação simples de pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

        if (!isExhausted && !isSprinting && Input.GetKey(KeyCode.LeftShift)) { Run(); }

        if (isExhausted && isSprinting || !Input.GetKey(KeyCode.LeftShift) && isSprinting) { StopRunning(); }

        if (!isSprinting && currentStamina < maxStamina) { RecoverStamina(); }

        if (currentStamina <= 0) { isExhausted = true; Debug.Log("Você está exausto."); }

        if (currentStamina >= maxStamina && !isRested) { isRested = true; Debug.Log("Você está descansado."); }

        if (isSprinting && currentStamina > 0) { ConsumeStamina(); }
        

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Run()
    {
        isSprinting = true; Debug.Log("Você está correndo.");
        movementSpeed *= sprintScale;
    }

    void StopRunning()
    {
        isSprinting = false; Debug.Log("Você parou de correr.");
        movementSpeed /= sprintScale;
    }

    void ConsumeStamina()
    {
        currentStamina -= Time.deltaTime; Debug.Log("Sua estamina está sendo consumida.");
    }

    void RecoverStamina()
    {
        currentStamina += staminaRecoverSpeed * Time.deltaTime; Debug.Log("Sua estamina está sendo recuperada.");
        if (currentStamina > maxStamina / 4 && isExhausted) { isExhausted = false; Debug.Log("Você não está mais exausto."); }
    }

    // Detecta colisões com itens
    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.CompareTag("Item"))
        {
            Destroy(triggerCollider.gameObject);
            OnItemPickup?.Invoke();
        }
    }
}
