#include<led_rgb.h>
#include<cores_rgb.h>

led_rgb status_led(11, 12, 13); //R_pin,G_pin,B_pin

void setup() {
  // put your setup code here, to run once:
  status_led.init();
}

void loop() {
  // put your main code here, to run repeatedly:
  status_led.acender(LED_COLOR_WHITE);
  delay(1000);
  status_led.acender(LED_COLOR_RED);
  delay(1000);
  status_led.acender(LED_COLOR_YELLOW);
  delay(1000);
  status_led.acender(LED_COLOR_GREEN);
  delay(1000);
  status_led.acender(LED_COLOR_AQUA);
  delay(1000);
  status_led.acender(LED_COLOR_BLUE);
  delay(1000);
  status_led.acender(LED_COLOR_MAGENTA);
  delay(1000);
  status_led.acender(LED_COLOR_RED);
  delay(1000);
  status_led.apagar();
  delay(1000);
}
