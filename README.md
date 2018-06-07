# NRF Tools
Ferramentas para programação dos transceptor nrf24le01, nrf24l01 e nrf24le1.

## Sobre os dispositivos

O NRF24LE1 é um transceptor de rádio frequência produzido pela [Nordic Semicondutors](www.nordicsemi.com). Este dispositivo é capaz de se comunicar na banda de frequência ISM (2.4 GHz) com taxas de transmissão em ar de até 2 MBits/s.

A sua principal vantagem com relação a outros dispositivos é o seu **preço** (Podem ser encontrado por U$$2,72 no AliExpress [Anúncio](https://pt.aliexpress.com/item/2PCS-NRF24LE1-NRF24L01-MCU-Wireless-Transceiver-DIP-Wireless-Communication-Module-nE/32644282935.html)).
Além disso ele pode ser configurado com diferentes antenas, conseguindo se comunicar sem fim em maiores distâncias. Ele também possui um consumo de energia baixo e é fortemente configurável, permitindo diferentes topologias e tipos de rede.

É importante notar as diferencias entre os módulos disponíveis no mercado, são eles:

* **nRF24LE1**: Módulo com microcontrolador 8051 integrado. Este módulo é composto pela antena nRF24LE01 e o microcontrolador 8051. Para programar este dispositivo é utilizado a IDE **Keil C51** e para gravar o código pode ser utilizado o **mPro** ou o [**gravador open-source**](https://github.com/italogfernandes/nrf24le1/releases) desenvolvido abaixo.

![Imagem do NRF24LE1](https://d1xahwiwo4b49p.cloudfront.net/3077-large_default/4ghz-nrf24le1-nrf24l01-mcu-wireless-transceiver-module.jpg)

* **nRF24LE01**: Módulo para ser utilizado com o arduino ou outros microcontroladores. Possui duas principais versões no mercado:
  * NRF24L01 - Simples:
  ![Imagem do NRF24L01](https://static.usinainfo.com.br/5546-thickbox_default/modulo-transceptor-wireless-24ghz-wifi-nrf24l01.jpg)
  * Com SNA e PA: Possui um amplificador e uma antena
  ![Imagem do NRF24L01 DIFERENTE](http://modtronix.com.au/wp-content/uploads/wrl-nrf24l01-pa_n.jpg)

## Instalando as Ferramentas

São necessários os seguintes softwares:
* Compilador: [Keil C51 e nRFgo-SDK](./instalacao-compilador-keil.html).
* Gravador:
    1. [mPro](https://drive.google.com/file/d/1Kz1PrjCGG2g5PH3JMu205yZxUusiQDPu/view).
    1. [Gravador Open-Source](./instalacao-gravador-open-source.html).
* Arduino IDE para Arduino Due e Bibliotecas: [Ferramentas arduino](./instalacao-arduino-bibliotecas.html).


[voltar](./)
### Autores

* **Ítalo Fernandes** - github.com/italogsfernandes
* **Talles Silva** - github.com/TallesSilva
