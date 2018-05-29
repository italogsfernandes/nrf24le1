#ifndef LED_RGB_H
#define LED_RGB_H

#include <Arduino.h>
#include <cores_rgb.h>

class led_rgb{
public:
  led_rgb(uint8_t red_pin, uint8_t green_pin, uint8_t blue_pin); //constructor
  bool led_status = 0; //Acesso ou apagado
  uint8_t RED_PIN = 11;  //Pino do arduino conectado ao led vermelho
  uint8_t GREEN_PIN = 12;  //Pino do arduino conectado ao led verde
  uint8_t BLUE_PIN = 13;  //Pino do arduino conectado ao led azul
  void acender(uint32_t color_vec);
  void apagar();
  void init();
private:
  unsigned long tempo, tempoAtual;
};

#endif
