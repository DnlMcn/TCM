using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autor: DnlMcn

public class Player : MonoBehaviour
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
    public float maxStamina = 15000;
    float currentStamina = 15000;
    float staminaRecoverSpeed = 5000; // Idealmente, gostaria que esse fosse um valor que você pudesse definir em segundos, 
    bool isExhausted;                 // que seria quantos segundos demora pra estamina preencher por completo.
    

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
    bool isGrounded;

    void Update()
    {
         // Verifica se o jogador está no chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reduz a velocidade vertical do jogador se ele encostar no chão
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;            
        }

        // Movimento básico com WASD
        if(isGrounded) // Faz com que o jogador só possa controlar a velocidade horizontal do personagem se ele estiver no chão
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 movement = transform.right * x + transform.forward * z;            
        }
        
        controller.Move(movement * movementSpeed * Time.deltaTime);

        // Implementação simples de um pulo
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Implementação simples de um botão de corrida
        if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            sprint = true;
            currentStamina -= Time.deltaTime;         
        }
        
        if (currentStamina <= 0) 
        {
            isExhausted = true;
            StopRunning();
        }
        

        if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded)
        {
            StopRunning();
        }

        if (sprint && !isSprinting && !isExhausted)
        {
            movementSpeed *= sprintScale;
            isSprinting = true;
        }
        
        if (

        // Aqui o Time.deltaTime é multiplicado novamente devido à equação geral da gravidade
        velocity.y += gravity * Time.deltaTime;

        // Movimenta o jogador com todas as implementações juntas
        controller.Move(velocity * Time.deltaTime);
    }

    void StaminaRecover()
    {
        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoverSpeed * Time.deltaTime;
        }        
    }

    void StopRunning()
    {
            sprint = false;
            isSprinting = false;
            movementSpeed /= sprintScale;
            StaminaRecover()
            
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.CompareTag("Item"))
        {
            Destroy(triggerCollider.gameObject);
            OnItemPickup?.Invoke();

        }
    }
}
