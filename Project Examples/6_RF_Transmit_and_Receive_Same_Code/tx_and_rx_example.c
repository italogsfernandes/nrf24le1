/* Minucurso de nRF24LE1 - 09/06/2018

Configurar as saidas
Configurar as entradas
Configurar a rede

Receber o comando do botão
Enviar um pacote
Receber um pacote
Acender os leds
*/
//Biblioteca dos registradores
#include<Nordic\reg24le1.h>
//Biblioteca de delay
#include<hal_delay.h>
//Biblioteca para comunicação RF
#include<nRF-SPICommands.h>
#include<stdint.h> //inteiros de 8 bits e etc
//C:\Nordic Semiconductor\nRFgo SDK 2.3.0.10040\source_code\hal

uint8_t code endereco_italo[5] = { 0x00, 0x00, 0x00, 0x00, 0x42}; 
uint8_t code endereco_cristian[5] = { 0x00, 0x00, 0x00, 0x00, 0x27};

void setup(){
	// Configurar os pinos
	/*
	LED Vermelho | P03
	LED Verde | P06

	Botão 1 | Reset
	Botão 2 | P14
	Botão 3 | P02
	*/
	P0DIR = 0xFF; // Tudo entrada
	
	P0DIR &= ~(1<<2); //P03 -> 0 -> OUTPUT
	P0DIR &= ~(1<<3); //P06 -> 0 -> OUTPUT
		
	P0DIR |= 1<<4; //P14 -> 1 -> INPUT
	P0DIR |= 1<<5; //P02 -> 2 -> INPUT
	
	rf_init(endereco_italo, endereco_italo,
					92, RF_DATA_RATE_2Mbps, RF_TX_POWER_0dBm);
}

void loop(){
	if(P04 == 0){ //Botão 1 pressionado
		//P03 = !P03; // Inverte LED
		tx_buf[0] = 'G'; // Green
		TX_Mode_NOACK(1); //G -> End | 'G' | CRC
		delay_ms(250);
	}
	if(P05 == 0){ //Botão 2 pressionado
		//P06 = !P06;
		tx_buf[0] = 'R'; //Red
		TX_Mode_NOACK(1); //R -> End | 'R' | CRC
		delay_ms(250);
	}
	if(newPayload){
		newPayload = 0;
		switch(rx_buf[0]){
			case 'G':
				P02 = !P02;
				break;
			case 'R':
				P03 = !P03;
				break;
			default:
				P02 = !P02; P03 = !P03; delay_ms(500);
				P02 = !P02; P03 = !P03; delay_ms(500);
				break;
		}
	}
}
void main(){
	setup();
	while(1){
		loop();
	}
}