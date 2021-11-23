using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float movementSpeed;
    public float gravity;
    public float jumpHeight;

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

        // Anula a velocidade do jogador se ele encostar no chão
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        // Movimento básico com WASD
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement * movementSpeed * Time.deltaTime);

        // Implementação simples de um pulo
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        // Aqui o Time.deltaTime é multiplicado novamente devido à equação geral da gravidade
        velocity.y += gravity * Time.deltaTime;

        // Movimenta o jogador com todas as implementações juntas
        controller.Move(velocity * Time.deltaTime);
    }
}
