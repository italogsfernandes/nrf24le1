# NRF Tools
Ferramentas para programação dos transponder nrf24le01, nrf24l01 e nrf24le1.

## Sobre os dispositivos

O NRF24LE1 é um transponder de rádio frequência produzido pela [Nordic Semicondutors](www.nordicsemi.com). Este dispositivo é capaz de se comunicar na banda de frequência ISM (2.4 GHz) com taxas de transmissão em ar de até 2 MBits/s.

É importante notar as diferencias entre os módulos disponívels no mercado, são eles:

* **nRF24LE1**: Módulo com microcontrolador 8051 integrado. Este módulo é composto pela antena nRF24LE01 e o microcontrolador 8051. Para programar este dispositivo é utilizado a IDE **Keil C51** e para gravar o código pode ser utilizado o **mPro** ou o [**gravador open-source**](https://github.com/italogfernandes/nrf24le1/releases) desenvolvido abaixo.

![Imagem do NRF24LE1](https://d1xahwiwo4b49p.cloudfront.net/3077-large_default/4ghz-nrf24le1-nrf24l01-mcu-wireless-transceiver-module.jpg)

* **nRF24LE01**: Módulo para ser utilizado com o arduino ou outros microcontroladores. Possui duas principais versões no mercado:
  * NRF24L01 - Simples:
  ![Imagem do NRF24L01](https://static.usinainfo.com.br/5546-thickbox_default/modulo-transceptor-wireless-24ghz-wifi-nrf24l01.jpg)
  * Com SNA e PA: Possui um amplificador e uma antena
  ![Imagem do NRF24L01 DIFERENTE](http://modtronix.com.au/wp-content/uploads/wrl-nrf24l01-pa_n.jpg)

## Instalando as Ferramentas

### Arduino IDE

1. Baixe o isntalador para o seu sistema no link [Arduino - Download](https://www.arduino.cc/en/Main/Software).
![Arduino Download](arduino_download.JPG)
 **Obs**: Caso utilize linux siga as instruções em: https://www.arduino.cc/en/Guide/Linux.
 Ou para Windows: https://www.arduino.cc/en/Guide/Windows

 2. Após instalar instale a biblioteca...`(Vou fazer dps)`
 
## Programando o NRF24L01 com o Arduino Due

## Programando o NRF24LE1 com o Keil

## Exemplos de código

### Autores

* **Ítalo Fernandes** - github.com/italogfernandes
* **Talles Silva** - github.com/TallesSilva
