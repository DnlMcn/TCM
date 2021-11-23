using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    // Sensitividade do mouse
    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    void Start()
    {
        // Prende o cursor no centro da tela e o torna invisível enquanto o jogo está rodando
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        // Adquire as informações de cada eixo de movimento
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Implementação para a rotação vertical da câmera 
        // (clamp é usado para que o jogador não possa virar para trás mais do que o natural)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Implementação da rotação horizontal da câmera
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
