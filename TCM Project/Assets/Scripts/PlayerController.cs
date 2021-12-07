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
    bool isExhausted;                        // que seria quantos segundos demora pra estamina preencher por completo.
    

    // Indica se o jogador está tentando correr
    bool sprint;
    // Verifica se o personagem já está correndo
    bool isSprinting;

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


        // Movimento e corrida

        // Movimento básico com WASD
        if(isGrounded) // Faz com que o jogador só possa controlar a velocidade horizontal do personagem se ele estiver no chão
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            movement = transform.right * x + transform.forward * z;            
        }
        controller.Move(movement * movementSpeed * Time.deltaTime);

        // Detecta se o jogador está correndo
        if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            sprint = true;
            currentStamina -= Time.deltaTime;         
        }

        // Detecta se o jogador está iniciando uma corrida
        if (sprint && !isSprinting && !isExhausted)
        {
            movementSpeed *= sprintScale;
            isSprinting = true;
        }

        // Detecta se a estamina do jogador acabou
        if (currentStamina <= 0) 
        {
            Debug.Log("You are exhausted.");
            isExhausted = true;
            StopRunning();
        }
        
        // Detecta se o jogador soltou a tecla de correr
        if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded && isSprinting)
        {
            sprint = false;
            StopRunning();
        }

        // Aqui o Time.deltaTime é multiplicado novamente devido à equação geral da gravidade
        velocity.y += gravity * Time.deltaTime;

        // Movimenta o jogador com todas as implementações juntas
        controller.Move(velocity * Time.deltaTime);
    }

    // Calcula o gasto de estamina
    void StaminaSpend()
    {

    }

    // Gradualmente aumenta currentStamina desde a mesma seja menor que maxStamina
    void StaminaRecover()
    {
        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoverSpeed * Time.deltaTime;
        }
        isExhausted = false;
        Debug.Log("Your stamina is fully recovered.");
    }

    // Executa as funções necessárias para que o personagem pare de correr
    void StopRunning()
    {
            movementSpeed /= sprintScale;
            StaminaRecover();
            isSprinting = false;
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
