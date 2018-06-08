# Blink - Exemplo para o nRF24LE1
Esse exemplo pisca um LED.

## Pre-requisitos
* Ter instalado os softwares para desenvolvimento.
* Saber criar um projeto e adicionar as biblitoecas HAL.

## Código
```cpp
#include<Nordic\reg24le1.h> // Registradores do nrf24le1
#include<hal_delay.h> // HAL para as funções de delay

/**
 * Função de inicialização das funções que serão utilizadas
 */
void setup(){
    // Inicializa os pinos do Port 0 e Port 1 todos como entradas.
    P0DIR = 0xFF; P1DIR = 0xFF;
    // Configura o pino 3 do Port 0 como saída
    P0DIR &= ~(1 << 3);
}

/**
 * Está função é chamada repetidivamente.
 */
void loop(){
    // Inverte o Pino 0 do Port 0
    P00 = !P00;
    // Aguarda 1s
    delay_ms(1000);
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

## Explicação

### P0DIR

O registrador P0DIR é responsável por determinar se um pino será
uma entrada (1) ou uma saída (0), sendo que cada bit dele equivale
a um pino do Port 0.
Por exemplo, se `P0DIR = 0xF7` (em binário 1111 0111), então:

Bit | Pino | Valor | Estado
--- | ---- | ----- | -----
0 | P00 | 1 | INPUT
1 | P01 | 1 | INPUT
2 | P02 | 1 | INPUT
3 | P03 | 0 | OUTPUT
4 | P04 | 1 | INPUT
5 | P05 | 1 | INPUT
6 | P06 | 1 | INPUT
7 | P07 | 1 | INPUT

### Hexadecimais
É comum o uso de hexadecimais para representar os valores, pois é rápida a
conversão de hexadecimal para binário, cada digito hex equivale a um conjunto de
4 bits.

#### Tabela para conversão rápida
Hex | Binary
--- | ------
0 | 0000
1 | 0001
2 | 0010
3 | 0011
4 |	0100
5 |	0101
6 | 0110
7 | 0111
8 | 1000
9 | 1001
A | 1010
B | 1011
C | 1100
D | 1101
E | 1110
F | 1111
10 | 0001 0000
20 | 0010 0000
40 | 0100 0000
80 | 1000 0000

### Operadores BitWise
Para manipulação de bits são utilizados os operados bitwise.
São eles:
* `&`: And
`0b10011110 & 0b11110111 = 0b10010110`
* `|`: Or
`0b10011110 | 0b00100000 = 0b10111110`
* '~': Not
`~0b00001000 = 0b11110111`
* '<': Left Shift
`1<3 = 0b100 = 0b00001000`
* '>': Right Shift
`0b100>3 = 0b1 = 0b00000001`

**Mascáras**

* Para setar um único bit de um byte para 1 sem alterar os outros você deve:
```cpp
original = 0x9E;
// 1º modo: Primeiro modo
original = original | 0x20;
// 2º modo: Utilizando um operador shift para facilitar
original = original | 1<<5;
// 3º modo: Simplificando
original |= 1<<5;
```

* Para setar um único bit de um byte para 0 sem alterar os outros você deve:
```cpp
original = 0x9E;
// 1º modo: Primeiro modo
original = original & 0xF7;
// 2º modo: Utilizando um operador not
original = original & ~(0x08);
// 2º modo: Utilizando um operador shift para facilitar
original = original & ~(1<<3);
// 3º modo: Simplificando
original &= ~(1<<3);
```

[home](../)
