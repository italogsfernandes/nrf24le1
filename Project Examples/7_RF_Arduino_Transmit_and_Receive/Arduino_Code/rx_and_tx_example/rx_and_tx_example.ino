#include "nrf24le01Module.h"
#include "led_rgb.h"

// Objetos
led_rgb status_led(7, 6, 5); //R_pin,G_pin,B_pin
nrf24le01Module host_nrf(2, 4, 3); //IRQ, CE, CSN

// Endereço de transissão, pode ser qualquer conjunto de 5 números
// Coloquei 0x42 por que 42 é um belo número
uint8_t endereco_tx[5] = {0x42, 0x42, 0x42, 0x42, 0x42};
// O endereço de recepção pode ser o mesmo de transmissão caso deseje
// Eu coloquei 0x2A por que é 42 em hexadecimal e acho 42 um número legal
uint8_t endereco_rx[5] = {0x2A, 0x2A, 0x2A, 0x2A, 0x2A};

void setup() {
  // Comunicação Serial
  Serial.begin(9600);

  // LED RGB que vem junto com a shield
  status_led.init();
  
  // Endereço RX, Endereço Tx, Canal, Data Rate, Power
  // O canal pode ser um numero entre 0 e 127, escolhi 92, escolher um canal diferente pode
  // ajudar a evitar interferência de outros dipositivos de 2.4GHz
  // 0dBm é a maior potência dentre as disponívels
  host_nrf.rf_init(endereco_tx, endereco_rx, 92, RF_DATA_RATE_2Mbps, RF_TX_POWER_0dBm); //RF Communication
  // Interupção de: Novos dados, dados enviados ou erro
  attachInterrupt(digitalPinToInterrupt(host_nrf.RFIRQ), rf_interrupt, FALLING); //Todo: put inside library
  
  // Mensagem Inicial
  Serial.println("Exemplo de transmissão e recepção com dispositivos nRF.");
  Serial.println("Envie L para ligar ou D para desligar um led a distância.");
}

void loop() {
  if (Serial.available()) {
    host_nrf.tx_buf[0] = Serial.read();
    host_nrf.TX_Mode_NOACK(1);
    Serial.print("Valor Enviado: ");
    Serial.println(host_nrf.tx_buf[0]);
  }

  if (host_nrf.newPayload) {
    host_nrf.newPayload = 0;
    Serial.print("Valor Recebido: ");
    Serial.println(host_nrf.rx_buf[0]);
    switch (host_nrf.rx_buf[0]) {
      case 'G':
        status_led.acender(LED_COLOR_GREEN);
        break;
      case 'B':
        status_led.acender(LED_COLOR_BLUE);
        break;
      default:
        status_led.acender(LED_COLOR_RED);
        break;
    }
  }
}

void rf_interrupt() {
  host_nrf.RF_IRQ();
}
