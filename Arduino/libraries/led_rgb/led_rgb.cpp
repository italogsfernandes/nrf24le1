#include "led_rgb.h"


led_rgb::led_rgb(uint8_t red_pin, uint8_t green_pin, uint8_t blue_pin){
  RED_PIN = red_pin;
  GREEN_PIN = green_pin;
  BLUE_PIN = blue_pin;
}

void led_rgb::apagar(){
    digitalWrite(RED_PIN,LOW); digitalWrite(GREEN_PIN,LOW); digitalWrite(BLUE_PIN,LOW);
    led_status = 0;
}

void led_rgb::acender(uint32_t color_vec){
    analogWrite(RED_PIN, (uint8_t) (color_vec>>16));
    analogWrite(GREEN_PIN, (uint8_t) (color_vec>>8));
    analogWrite(BLUE_PIN, (uint8_t) (color_vec));
    led_status =  color_vec != 0;
}

void led_rgb::init(){
    pinMode(RED_PIN,OUTPUT); pinMode(GREEN_PIN,OUTPUT); pinMode(BLUE_PIN,OUTPUT);
}

