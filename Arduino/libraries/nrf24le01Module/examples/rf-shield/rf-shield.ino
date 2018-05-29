#include "nrf24le01Module.h"
#include "led_rgb.h"

#define BROADCAST_ADDR  0x00

#define UART_BAUDRATE       115200


led_rgb status_led(7, 6, 5); //R_pin,G_pin,B_pin
nrf24le01Module host_nrf(2, 4, 3); //IRQ, CE, CSN

char serialOp;
uint16_t acc[3];
uint16_t gyr[3];
uint16_t mag[3];
uint16_t tmp;

unsigned long received_millis, sent_millis , timeout_millis, actual_millis;
void setup() {
  Serial.begin(UART_BAUDRATE);
  status_led.init();
  host_nrf.rf_init(host_nrf.ADDR_HOST, host_nrf.ADDR_HOST, 30, RF_DATA_RATE_2Mbps, RF_TX_POWER_0dBm); //RF Communication
  attachInterrupt(digitalPinToInterrupt(host_nrf.RFIRQ), rf_interrupt, FALLING); //Todo: put inside library
  piscas_iniciais_led();
  Serial.println("Host Iniciado.");
}

void loop() {
  actual_millis = millis();
  if (host_nrf.newPayload) {
    received_millis = actual_millis;
    host_nrf.newPayload = 0;
    status_led.acender(LED_COLOR_AQUA);
    if (host_nrf.payloadWidth != 20) {
      Serial.println("**Payload Recebida**");
      Serial.print("Payload Width: "); Serial.println(host_nrf.payloadWidth);
      Serial.print("Payload Data: "); Serial.write(host_nrf.rx_buf, host_nrf.payloadWidth);
      Serial.print("\n");
      for (int i = 0; i < host_nrf.payloadWidth; i++) {
        Serial.print("0x"); Serial.print(host_nrf.rx_buf[i], HEX); Serial.print(" ");
      }
      Serial.print("\n");
    }
    if (host_nrf.payloadWidth == 20) {
      acc[0] = host_nrf.rx_buf[ 0] << 8 | host_nrf.rx_buf[ 1];
      acc[1] = host_nrf.rx_buf[ 2] << 8 | host_nrf.rx_buf[ 3];
      acc[2] = host_nrf.rx_buf[ 4] << 8 | host_nrf.rx_buf[ 5];

      gyr[0] = host_nrf.rx_buf[ 6] << 8 | host_nrf.rx_buf[ 7];
      gyr[1] = host_nrf.rx_buf[ 8] << 8 | host_nrf.rx_buf[ 9];
      gyr[2] = host_nrf.rx_buf[10] << 8 | host_nrf.rx_buf[11];

      mag[0] = host_nrf.rx_buf[12] << 8 | host_nrf.rx_buf[13];
      mag[1] = host_nrf.rx_buf[14] << 8 | host_nrf.rx_buf[15];
      mag[2] = host_nrf.rx_buf[16] << 8 | host_nrf.rx_buf[17];

      tmp = host_nrf.rx_buf[18] << 8 | host_nrf.rx_buf[19];

      Serial.print(acc[0]); Serial.print("\t");
      Serial.print(acc[1]); Serial.print("\t");
      Serial.print(acc[2]); Serial.print("\t");
      Serial.print("-"); Serial.print("\t");

      Serial.print(gyr[0]); Serial.print("\t");
      Serial.print(gyr[1]); Serial.print("\t");
      Serial.print(gyr[2]); Serial.print("\t");
      Serial.print("-"); Serial.print("\t");

      Serial.print(mag[0]); Serial.print("\t");
      Serial.print(mag[1]); Serial.print("\t");
      Serial.print(mag[2]); Serial.print("\t");
      Serial.print("-"); Serial.print("\t");

      Serial.print((float) ((tmp / 340.00) + 36.53), 2); Serial.print("\n");
      //Serial.print("\n");
    }
  }


  if (Serial.available() >= 2) {
    sent_millis = actual_millis;
    status_led.acender(LED_COLOR_YELLOW);
    host_nrf.tx_buf[0] = Serial.read();
    host_nrf.tx_buf[1] = Serial.read();
    host_nrf.TX_Mode_NOACK(2);
    Serial.println("**Payload Enviada**");
    Serial.print("Payload Width: "); Serial.println(2);
    Serial.print("Payload Data: "); Serial.write(host_nrf.tx_buf, 2);
    Serial.print("\n");
    for (int i = 0; i < 2; i++) {
      Serial.print("0x"); Serial.print(host_nrf.tx_buf[i], HEX); Serial.print(" ");
    }
    Serial.print("\n");
  }
  if (actual_millis - received_millis  > 250) {
    status_led.apagar();
  }
  if (actual_millis - sent_millis > 250) {
    status_led.apagar();
  }
}

void rf_interrupt() {
  host_nrf.RF_IRQ();
}

void piscas_iniciais_led() {
  status_led.acender(LED_COLOR_RED); delay(500);
  status_led.apagar(); delay(500);
  status_led.acender(LED_COLOR_GREEN); delay(500);
  status_led.apagar(); delay(500);
  status_led.acender(LED_COLOR_BLUE); delay(500);
  status_led.apagar(); delay(500);
}


