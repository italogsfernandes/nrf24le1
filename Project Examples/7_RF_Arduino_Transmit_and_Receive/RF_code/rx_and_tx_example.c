#include<Nordic\reg24le1.h> // Registradores do nrf24le1
#include<hal_delay.h> // HAL para as funções de delay
#include "nRF-SPICommands.h" // Comunicacao RF, desenvolvida pelo prof. Sergio Ricardo de Jesus Oliveira

// Endereço de recepção, pode ser qualquer conjunto de 5 números
// Coloquei 0x42 por que 42 é um belo número
uint8_t code endereco_nrf24le1[5] = {0x42, 0x42, 0x42, 0x42, 0x42};
// O endereço de transmissão pode ser o mesmo de transmissão caso deseje
// Eu coloquei 0x2A por que é 42 em hexadecimal e acho 42 um número legal
uint8_t code endereco_arduino[5] = {0x2A, 0x2A, 0x2A, 0x2A, 0x2A};


void setup(){
    // Inicializa os pinos do Port 0 e Port 1 todos como entradas.
    P0DIR = 0xFF; P1DIR = 0xFF;
    // Configura o pino 3 do Port 0 como saída
    P0DIR &= ~(1 << 3);
    // Configura os pinos 4 e 5 como entradas
    P0DIR != 1 << 4;
    P0DIR != 1 << 5;

    // Inicializa a comunicacao por radio frequencia
    rf_init(endereco_arduino, endereco_nrf24le1, 92, RF_DATA_RATE_2Mbps, RF_TX_POWER_0dBm);
}

void loop(){
    if(!P04){
        tx_buf[0] = 'G';
        TX_Mode_NOACK(1);
    }

    if(!P05){
        tx_buf[0] = 'G';
        TX_Mode_NOACK(1);
    }

    if(newPayload){
        newPayload = 0;
        switch(rx_buf[0]){
            case 'L':
            P03 = 1;
            break;
            case 'D':
            P03 = 0;
            break;
            default:
            P03 = !P03; delay_ms(500);
            P03 = !P03; delay_ms(500);
            break;
        }
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