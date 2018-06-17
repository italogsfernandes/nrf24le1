#include "nrf24le01Module.h" //
#include "led_rgb.h"

led_rgb status_led(7, 6, 5); //R_pin,G_pin,B_pin
nrf24le01Module host_nrf(2, 4, 3); //IRQ, CE, CSN

void setup() {
  Serial.begin(9600);
  host_nrf.rf_init(host_nrf.ADDR_HOST, host_nrf.ADDR_HOST, 92, RF_DATA_RATE_2Mbps, RF_TX_POWER_0dBm); //RF Communication
  //attachInterrupt(digitalPinToInterrupt(host_nrf.RFIRQ), rf_interrupt, FALLING); //Todo: put inside library
  Serial.println("Host Iniciado.");
}

void loop() {
  if (Serial.available()) {
    host_nrf.tx_buf[0] = Serial.read();
    if (host_nrf.tx_buf[0] == '1') {
      host_nrf.tx_buf[0] = 'O';
      host_nrf.tx_buf[1] = 'L';
      host_nrf.tx_buf[2] = 'A';
      host_nrf.TX_Mode_NOACK(3);
    } else {
      host_nrf.TX_Mode_NOACK(1);
      Serial.print("Valor enviado: ");
      Serial.println((char) host_nrf.tx_buf[0]);
    }

  }
  if (host_nrf.newPayload) {
    host_nrf.newPayload = 0;
    Serial.print("Valor recebido: ");
    Serial.println((char) host_nrf.rx_buf[0]);
    if (host_nrf.rx_buf[0] == 'G') {
      status_led.acender(LED_COLOR_GREEN);
    } else if (host_nrf.rx_buf[0] == 'R') {
      status_led.acender(LED_COLOR_RED);
    }
  }
  if (!digitalRead(host_nrf.RFIRQ)) {
    host_nrf.RF_IRQ();
  }
}
