# Instalação do Gravador do nRF24LE1

 Para instalar o gravador antes instale o Gtk#.
 O Gravador open source pode ser instalado a partir do anexo depositado no email.

1. Acesse o site de downloads do mono:  http://www.mono-project.com/download/stable/
![image](https://user-images.githubusercontent.com/21316901/41117639-f678ff1a-6a63-11e8-93f5-36e2033091c7.png)

2. Baixe o instalador: **GTK# for .NET** - Installer for running Gtk#-based applications on Microsoft .NET
![image](https://user-images.githubusercontent.com/21316901/41117649-00484ad2-6a64-11e8-8a58-12556d78eb9c.png)

3. Prossiga com a instalação até o final.                   
![image](https://user-images.githubusercontent.com/21316901/41117665-09d831fc-6a64-11e8-9d91-9d43a51d7d10.png)

4. Após instalar, extraia o arquivo **WindowsRelease.rar** execute o arquivo **nrf-Burner**.
![image](https://user-images.githubusercontent.com/21316901/41117679-13d11854-6a64-11e8-8756-26c2a651cb98.png)

[voltar](./instalacao.html)

# Passo a Passo de como Gravar
Após toda instalação do keil e também do gravador, você se encontra apto para iniciar o processo de gravação do código.
1. Ligação Arduino Due e nRF24LE1

![image](https://github.com/italogsfernandes/nrf24le1/blob/master/docs/img/esquema_Due_nrf.jpg?raw=true)
2. Após montar o circuito de programação do Due e do NRF você precisa gerar o arquivo .hex do seu código e copiar ele para o compilador do gravador. Se caso o gravador não funcionar para compilar o .hex utilizar o "codigo-arduino-gravador-nrf.ino".
![image](https://github.com/italogsfernandes/nrf24le1/blob/master/docs/img/codigo-gravador-due.PNG?raw=true)
3. Para gravar usando o codigo é necessario compilar e gravar no arduino, além disso abrir o serial e digitar "S8"
4. Já no gravador, precisaremos apenas de conectar e executar a opção de compilação, assim todos os passos serão feitos automaticamente.
![image](https://github.com/italogsfernandes/nrf24le1/blob/master/docs/img/Gravador.PNG?raw=true)

[voltar](./instalacao.html)
