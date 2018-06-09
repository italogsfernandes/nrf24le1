#include<Nordic\reg24le1.h> // registradores

void main(){
	//configurações
	P0DIR = 0x00; // 0 - Saida e 1 - Entrada
	while(1){
		//Loop
		P03 = !P03;
	}
}