# NRF Tools
Ferramentas para programação dos transceptor nrf24le01, nrf24l01 e nrf24le1.

## Sobre os dispositivos

O NRF24LE1 é um transceptor de rádio frequência produzido pela [Nordic Semicondutors](www.nordicsemi.com). Este dispositivo é capaz de se comunicar na banda de frequência ISM (2.4 GHz) com taxas de transmissão em ar de até 2 MBits/s.

É importante notar as diferencias entre os módulos disponíveis no mercado, são eles:

* **nRF24LE1**: Módulo com microcontrolador 8051 integrado. Este módulo é composto pela antena nRF24LE01 e o microcontrolador 8051. Para programar este dispositivo é utilizado a IDE **Keil C51** e para gravar o código pode ser utilizado o **mPro** ou o [**gravador open-source**](https://github.com/italogfernandes/nrf24le1/releases) desenvolvido abaixo.

![Imagem do NRF24LE1](https://d1xahwiwo4b49p.cloudfront.net/3077-large_default/4ghz-nrf24le1-nrf24l01-mcu-wireless-transceiver-module.jpg)

* **nRF24LE01**: Módulo para ser utilizado com o arduino ou outros microcontroladores. Possui duas principais versões no mercado:
  * NRF24L01 - Simples:
  ![Imagem do NRF24L01](https://static.usinainfo.com.br/5546-thickbox_default/modulo-transceptor-wireless-24ghz-wifi-nrf24l01.jpg)
  * Com SNA e PA: Possui um amplificador e uma antena
  ![Imagem do NRF24L01 DIFERENTE](http://modtronix.com.au/wp-content/uploads/wrl-nrf24l01-pa_n.jpg)

## Instalando as Ferramentas

### NRF24L01 + Arduino DUE

1. Baixe o instalador para o seu sistema no link [Arduino - Download](https://www.arduino.cc/en/Main/Software).
![Arduino Download](arduino_download.JPG)
 **Obs**: Caso utilize linux siga as instruções em: https://www.arduino.cc/en/Guide/Linux.
 Ou para Windows: https://www.arduino.cc/en/Guide/Windows

2. Instale a Placa Arduino Due na IDE.
  * Acesse: Ferramentas -> Placas -> Gerenciador de Placas.
  * Pesquise Arduino Due e instale a opção apresentada.

3. Após instalar o arduino, baixe os arquivos deste repositório (link)[https://github.com/italogfernandes/nrf24le1],
então coloque os arquivos de Arduino/libraries na pasta ´Arduino/libraries´ do seu sistema.
![Arduino Download](arduino_libraries.PNG)

### NRF24L01 + STM32

1. Instale a IDE do arduino como descrito no passo 1 da sessão **NRF24L01 + Arduino DUE**.

2. Configure a IDE para programar o STM32.

3. Escreverei está parte mais tarde

### NRF24LE1 com Keil C51

1. Baixe e instale o **Keil C51**. Coloquei um tutorial a parte para este processo, tenha em mãos os meios de comprar uma licença do compilador caso você deseje fazer programas acima de 2 mil linhas. [Tutorial de Instalação do Keil C51](https://www.google.com/)

2. Baixe as bibliotecas fornecidas pela Nordic para o nRF24LE1, [nRFgo-SDK](http://www.nordicsemi.com/eng/Products/2.4GHz-RF/nRFgo-SDK).
![Imagem do nRFgo-SDK](nrfgo_sdk.png)

3. Instale o nRFgo-SDK. (Next-> I Agree-> Selecione **Keil uVision header files** -> Next-> Install -> Aguarde a Instalação -> Next -> Finish)
![Instalação do nrfgo-SDK](nrfgo_sdk_setup_3.PNG)

### Instalação do Gravador do nRF24LE1

Para utilizar como gravador existe o mPro ou o Gravador open-source.
Posteriormente colocarei aqui o passo a passo da instalação.

## Programando o NRF24L01 com o Arduino Due

## Programando o NRF24LE1 com o Keil

## Exemplos de código

### Autores

* **Ítalo Fernandes** - github.com/italogfernandes
* **Talles Silva** - github.com/TallesSilva
