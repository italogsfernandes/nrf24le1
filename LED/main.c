#include<Nordic\reg24le1.h>

void main(){
	P0DIR = 0x00;
	while(1){
		P03 = !P03;
	}
}