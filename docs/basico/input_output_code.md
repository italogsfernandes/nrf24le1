# GPIO - Exemplo para o nRF24LE1
Exemplo de uso das entradas e saídas, General Purpose Input/Output (GPIO).

## Pre-requisitos
* Ter instalado os softwares para desenvolvimento.
* Saber criar um projeto e adicionar as biblitoecas HAL.
* Exemplo do blink.

## Código

### Opção 1
```cpp
#include<Nordic\reg24le1.h> // Registradores do nrf24le1

/**
 * Função de inicialização das funções que serão utilizadas
 */
void setup(){
    // Inicializa os pinos do Port 0 e Port 1 todos como entradas.
    P0DIR = 0xFF; P1DIR = 0xFF;
    // Configura o pino 3 do Port 0 como saída
    P0DIR &= ~(1 << 3);
    P0DIR |= (1 << 4);
}

/**
 * Está função é chamada repetidivamente.
 */
void loop(){
    // Se o botão está pressionado
    if(!P04){ // Se P04 vale 0
        P03 = 1; // Acende o LED
    } else { // Caso o botão estiver solto
        P03 = 0; // Apaga o LED
    }
}

/**
 * Função Main, indicando como funcionará o sitema.
 * Ele está simplificado para funcionar semelhante ao arduino.
 */
void main(){
    setup();
    while(1){
        loop();
    }
}
```

### Opção 2
```cpp
#include<Nordic\reg24le1.h> // Registradores do nrf24le1

/**
 * Função de inicialização das funções que serão utilizadas
 */
void setup(){
    // Inicializa os pinos do Port 0 e Port 1 todos como entradas.
    P0DIR = 0xFF; P1DIR = 0xFF;
    // Configura o pino 3 do Port 0 como saída
    P0DIR &= ~(1 << 3);
    P0DIR |= (1 << 4);
}

/**
 * Está função é chamada repetidivamente.
 */
void loop(){
    if(!P04){ //Se o botão foi pressionado
        P03 = !P03; // Inverte o LED
        delay_ms(300);
    }
}

/**
 * Função Main, indicando como funcionará o sitema.
 * Ele está simplificado para funcionar semelhante ao arduino.
 */
void main(){
    setup();
    while(1){
        loop();
    }
}
```
## Conexões

PINO | Conexão
---- | -------
VCC  | Alimentação
GND  | Ground
P03  | LED com resistor
P04  | Botão com Pull-Up


[home](../)
