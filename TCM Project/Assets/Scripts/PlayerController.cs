using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: DnlMcn

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    // Velocidade de movimento base
    public float movementSpeed;
    // Valor que multiplica a velocidade base ao correr
    public float sprintScale;
    // Força da gravidade
    public float gravity;
    // Altura desejada do pulo
    public float jumpHeight;
    // Variáveis relacionando à estamina (em milisegundos);
    public float maxStamina;
    float currentStamina;
    public float staminaRecoverSpeed; // Idealmente, gostaria que esse fosse um valor que você pudesse definir em segundos, 
    bool isExhausted;                 // que seria quantos segundos demora pra estamina preencher por completo.
    bool isRested; 
    
    // Verifica se o personagem está correndo
    bool isSprinting;
    // Verifica se o jogador está tentando correr
    bool sprint;

    public event System.Action OnItemPickup;

    public Transform groundCheck;
    // Define a distância dos pés do jogador na qual o chão é detectado
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

        // Verifica se o jogador está no chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
        if (isGrounded) // Faz com que o jogador só possa controlar a velocidade horizontal do personagem se ele estiver no chão
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
        

        // Aqui o Time.deltaTime é multiplicado novamente devido à equação geral da gravidade
        velocity.y += gravity * Time.deltaTime;

        // Movimenta o jogador com todas as implementações juntas
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
