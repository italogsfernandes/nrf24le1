#include<Nordic\reg24le1.h>
#incude<hal_delay.h>

void setup(){
void main(){
	P0DIR = 0xff;//entrada
	P1DIR = 0xff;//entrada
	P0DIR = 0xef //setar bit4 do p0dir para 0
	}
}
void loop(){	
		P04 = 1;
		delay_ms(1000);
		P04 = 0;
		delay_ms(1000);
}


	while(1){
		loop()
	}