using System;
using Gtk;
using nrfBurner; //Classes do projeto
using System.IO.Ports; //Porta Serial - Comunicacao cm or arduino
using System.Threading; //Timer de leitura

public partial class MainWindow: Gtk.Window
{
	public program_data codigo_atual;
	public program_data codigo_lido;

	SerialPort minhaporta;
	FileFilter hex_files_filter; 
	FileFilter log_files_filter;

	public Thread mythr;
	public bool threadRunningFlag;
	public bool threadWorkingFlag;
	public bool timeoutflag;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		this.Title = "Gravador do nrf24le1";
		user_log ("Aplicação Iniciada.");

		minhaporta = new SerialPort ();
		mythr = new Thread (new ThreadStart (ThreadRoutine));
		mythr.Priority = ThreadPriority.Highest;

		StartThread ();

		lbl_status2.Text = "";

		this.logtextview.SizeAllocated += new SizeAllocatedHandler (Scroll2);

		hex_files_filter = new FileFilter();
		hex_files_filter.Name = ".hex files";
		hex_files_filter.AddPattern("*.hex");
		filechooserbutton1.AddFilter(hex_files_filter);

		log_files_filter = new FileFilter();
		log_files_filter.Name = ".burnerlog";
		log_files_filter.AddPattern("*.burnerlog");

		codigo_atual = new program_data ();

		//Definições da porta serial
		minhaporta.BaudRate = 9600;
		minhaporta.Parity = Parity.None;
		minhaporta.DataBits = 8;
		minhaporta.StopBits = StopBits.One;
		minhaporta.ReadTimeout = 2;
		minhaporta.ReadBufferSize = 1024;

		listar ();
	}

	private void ThreadRoutine ()
	{
		while (threadRunningFlag) { //thread will be always running
			if (threadWorkingFlag) { //do sommething or do not something
				//doing something
				SomeLongComputation();
			}
		}
	}
	private void wait_serial_bytes(int how_many){
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();
		while(minhaporta.IsOpen && minhaporta.BytesToRead < how_many)
		{
			if (sw.ElapsedMilliseconds > 500) {
				Console.WriteLine ("Timeout reached");
				break;
			}
		}
	
	}
	private void SomeLongComputation(){
		if (minhaporta.IsOpen && minhaporta.BytesToRead > 1) {
			int start_signal = minhaporta.ReadByte ();
			if (start_signal == 'S') {
				string textread = "";
				wait_serial_bytes (1);
				switch (minhaporta.ReadByte ()) {
				case 0xFF:
					textread = "Erro!!!!!!!";
					break;
				case 0xFE:
					textread = "Mensagem: ";
					wait_serial_bytes (1);
					int qnt = minhaporta.ReadByte ();
					wait_serial_bytes (qnt);
					char[] lidos = new char[qnt];
					minhaporta.Read (lidos, 0, qnt);
					textread += new string (lidos);
					break;
				case 0xFD:
					codigo_lido = new program_data ();
					textread = "Dados: ";
					wait_serial_bytes (1);
					minhaporta.ReadByte ();//addr msb
					for (int i = 0; i < 32; i++) {
						wait_serial_bytes (16);
						minhaporta.Read (codigo_lido.hex_code, i, 16);
					}
					Gtk.Application.Invoke (delegate {
						show_code (codigo_lido);
					});
					break;
				default:
					textread = "Sinal serial não reconhecido";
					break;
				}

				Gtk.Application.Invoke (delegate {
					user_log (textread);
				}); 

			} else {
				Gtk.Application.Invoke (delegate {
					txt_codigo.Buffer.Text += Convert.ToChar(start_signal).ToString();
				}); 
			}
		}
	}

	private void listar()
	{
		//listando as portas
		foreach (string item in SerialPort.GetPortNames())
		{
			cbPortasSeriais.AppendText (item);

		}
		cbPortasSeriais.Active = 0; //definindo a primeira como default
	}


	public void user_log(string mensagem){
		logtextview.Buffer.Text += "\n" + System.DateTime.Now.TimeOfDay.ToString ().Substring (0, 8) + " - "
			+ mensagem;
	}

	private void StartThread(){
		threadRunningFlag = true; //Running
		threadWorkingFlag = false; //but doing nothing
		mythr.Start();
	}

	private void RunThread(){
		threadWorkingFlag = true; //doing something
	}

	private void PauseThread(){
		threadWorkingFlag = false; //thread do not do what she used to do
	}

	private void StopThread(){
		threadWorkingFlag = false; //Dont do what she used to do
		threadRunningFlag = false; //Stop infinite lace
	}

	public void Scroll2(object sender, Gtk.SizeAllocatedArgs e)
	{
		logtextview.ScrollToIter(logtextview.Buffer.EndIter, 0, false, 0, 0);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		//sempre lembre-se de fechar a porta antes de sair do programa
		if(minhaporta.IsOpen){
			minhaporta.Close();
		}
		if (threadRunningFlag) {
			StopThread ();
		}
		if (mythr.IsAlive) {
			mythr.Abort ();
		}
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnFilechooserbutton1SelectionChanged (object sender, EventArgs e)
	{
		user_log ("Arquivo selecionado: " + filechooserbutton1.Filename.ToString ());

		System.IO.StreamReader file =
			System.IO.File.OpenText( filechooserbutton1.Filename );

		string str_codigo_hex= file.ReadToEnd();
		user_log ("Lido:\n" + str_codigo_hex);
		codigo_atual.carregar_arquivo (str_codigo_hex);
		user_log ("Arquivo lido com sucesso.");
		txt_codigo.Buffer.Clear ();
		show_code (codigo_atual);
		export_vector (codigo_atual);

		// Set the MainWindow Title to the filename.
		this.Title = "Gravador do nrf24le1 -- " + filechooserbutton1.Filename.ToString();
		file.Close(); // Close the file so as to not leave a mess.
	}

	private void show_code(program_data codigo){
		string str_out = "\t\t0x00 0x01 0x02 0x03 0x04 0x05 0x06 0x07 0x08 0x09 0x0A 0x0B 0x0C 0x0D 0x0E 0x0F\n\n".Replace(" ","\t");
		//str_out += "\t\t-------------------------------------------------------------------------------\n";
		for (int i = 0; i < Math.Ceiling((float)(codigo.hex_code.Length)/16); i++) {
			if (i < 16) {
				str_out += "0x0" + i.ToString("X") + ":\t";
			} else {
				str_out += "0x" + i.ToString("X") + ":\t";
			}

			for (int j = 0; j < 16; j++) {
				if (codigo.hex_code [i * 16 + j] < 16) {
					str_out += "0x0"+codigo.hex_code[i*16+j].ToString("X") + "\t";
				} else {
					str_out += "0x"+codigo.hex_code[i*16+j].ToString("X") + "\t";
				}
			}
			str_out += "\n";
		}
		txt_codigo.Buffer.Text += str_out;
	}

	private void export_vector(program_data codigo){
		string str_out = "\n***********************************************************************************\n" +
			"Copie o seguinte código em seu programa em C: \n\n" +
			"uint8_t hex_code[" + codigo.hex_code.Length.ToString () + "] = {\n";
		for (int i = 0; i < Math.Ceiling((float)(codigo.hex_code.Length)/16); i++) {
			for (int j = 0; j < 16; j++) {
				if (codigo.hex_code [i * 16 + j] < 16) {
					str_out += "0x0"+codigo.hex_code[i*16+j].ToString("X");
				} else {
					str_out += "0x"+codigo.hex_code[i*16+j].ToString("X");
				}
				if (j == 15 && i == Math.Ceiling((float)(codigo.hex_code.Length)/16)-1){
					str_out += " };";
				} else {
					str_out += ", ";
				}
			}
			str_out += "\n";
		}
		str_out += "\n***********************************************************************************\n";
		txt_codigo.Buffer.Text += str_out;
	}

	private void export_hex_commands (program_data codigo)
	{
		//codigo.hex_str;
		string str_out = "\n***********************************************************************************\n" +
			"O seguinte codigo representa os comandos hex a serem executados: \n\n" +
			"uint8_t hex_commands[" + codigo.hex_code.Length.ToString () + "] = {\n";
		for (int i = 0; i < Math.Ceiling ((float)(codigo.hex_code.Length) / 16); i++) {
			for (int j = 0; j < 16; j++) {
				if (codigo.hex_code [i * 16 + j] < 16) {
					str_out += "0x0" + codigo.hex_code [i * 16 + j].ToString ("X");
				} else {
					str_out += "0x" + codigo.hex_code [i * 16 + j].ToString ("X");
				}
				if (j == 15 && i == Math.Ceiling ((float)(codigo.hex_code.Length) / 16) - 1) {
					str_out += " };";
				} else {
					str_out += ", ";
				}
			}
			str_out += "\n";
		}
		str_out += "\n***********************************************************************************\n";
		txt_codigo.Buffer.Text += str_out;
	}

	protected void OnBtnConectarClicked (object sender, EventArgs e)
	{
		if (minhaporta.IsOpen) { //Se esta aberta desconecta

			user_log("Desconectando de " + minhaporta.PortName+ "...");

			try {
				minhaporta.Close();
				PauseThread();
				user_log("Desconectado.");
				cbPortasSeriais.Sensitive = true;
				btnConectar.Label = "Conectar";
				btnConectar.Image = new Image(Stock.Connect, IconSize.Button);
			} catch (Exception ex){
				user_log ("Algo deu errado ao desconectar.\nErro: " + ex.Message);
				cbPortasSeriais.Sensitive = false;
			}
		} else { //se esta fechada se conecta

			minhaporta.PortName = cbPortasSeriais.ActiveText.ToString();
			user_log("Conectando a " + cbPortasSeriais.ActiveText.ToString() + "...");

			//tenta se conectar, caso ocorra algm erro o captura e mostra
			try {
				//define a porta a se conectar como a selecionada
				minhaporta.Open();
				user_log ("Conectado.");
				RunThread();
				cbPortasSeriais.Sensitive = false;
				btnConectar.Label = "Desconectar";
				btnConectar.Image = new Image(Stock.Disconnect, IconSize.Button);
			} catch (Exception ex){
				user_log ("Não foi possível conectar.\nErro: " + ex.Message);
				cbPortasSeriais.Sensitive = true;
			}
		}

	}
	public void try_to_send(String something){
		try {
			minhaporta.Write(something);
		} catch (Exception ex){
			user_log ("Não foi possível enviar dados.\nErro: " + ex.Message);
		}
	}
	public void try_to_send(byte something){
		byte[] enviar = { something };
		try {
			minhaporta.Write(enviar,0,1);
		} catch (Exception ex){
			user_log ("Não foi possível enviar dados.\nErro: " + ex.Message);
		}
	}
	public void try_to_send(char something){
		char[] enviar = { something };
		try {
			minhaporta.Write(enviar,0,1);
		} catch (Exception ex){
			user_log ("Não foi possível enviar dados.\nErro: " + ex.Message);
		}
	}
	protected void OnBtnBurnClicked (object sender, EventArgs e)
	{
		/*
		[x] Entrar no modo de programação.
		[x] Conectar ao dispositivo.
		[x] Apagar código antigo.
		[x] Escrever código novo.
		[x] Ler código escrito.
		[x] Comparar para verificar.
		[x] Sair do modo de programação.
		*/
		filechooserbutton1.Sensitive = false;
		btnBurn.Sensitive = false;
		sendAutomaticModeSignal ();
		/*send_hex_code ();
		iniciar_prog_mode ();
		conectar_prog_mode ();
		requisitar_info_page ();
		apagar_old_code ();
		write_actual_code ();
		verify_code ();
		desconectar_prog_mode ();*/
		btnBurn.Sensitive = true;
		filechooserbutton1.Sensitive = true;
	}

	protected void send_hex_code(){
		try_to_send('S');
		try_to_send(0x00);
		try_to_send(Convert.ToByte(codigo_atual.hex_code.Length>>8));
		try_to_send(Convert.ToByte(codigo_atual.hex_code.Length));

		foreach (var linha in codigo_atual.hex_str.Split()) {
			if (linha.StartsWith (":")) {
				send_linha_hex(new comando_cod_hex (linha));
			}
		}
	}

	protected void send_linha_hex(comando_cod_hex linha2send){
		try_to_send (':');
		foreach (byte item in linha2send.get_vector_hex()) {
			try_to_send (item);
		}
	}

	void iniciar_prog_mode (){
		try_to_send ('S');
		try_to_send (0x01);
	}
	void conectar_prog_mode (){
		try_to_send ('S');
		try_to_send (0x02);
	}
	void requisitar_info_page (){
		try_to_send ('S');
		try_to_send (0x03);
	}
	void apagar_old_code (){
		try_to_send ('S');
		try_to_send (0x04);
	}
	void write_actual_code (){
		try_to_send ('S');
		try_to_send (0x05);
	}
	void verify_code (){
		try_to_send ('S');
		try_to_send (0x06);
	}
	void desconectar_prog_mode (){
		try_to_send ('S');
		try_to_send (0x07);
	}
	protected void sendAutomaticModeSignal(){
		try_to_send('S');
		try_to_send(0x08);
	}
	void readFlashCode (){
		try_to_send ('S');
		try_to_send (0x09);
	}
	protected void OnBtnSendHexClicked (object sender, EventArgs e)
	{
		try_to_send ('S');
		try_to_send (0x00);
		//send_hex_code ();
	}

	protected void OnBtnIniciarClicked (object sender, EventArgs e)
	{
		iniciar_prog_mode ();
	}

	protected void OnBtnProgModeClicked (object sender, EventArgs e)
	{
		conectar_prog_mode ();
	}

	protected void OnBtnReadInfoClicked (object sender, EventArgs e)
	{
		requisitar_info_page ();
	}

	protected void OnBtnEraseClicked (object sender, EventArgs e)
	{
		apagar_old_code ();
	}

	protected void OnBtnWriteNewClicked (object sender, EventArgs e)
	{
		write_actual_code ();
	}

	protected void OnBtnVerifyClicked (object sender, EventArgs e)
	{
		verify_code ();
	}

	protected void OnBtnExitProgClicked (object sender, EventArgs e)
	{
		desconectar_prog_mode ();
	}

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{
		// Reset the logTreeView and change the window back to original size
		int width, height;
		this.GetDefaultSize( out width, out height );
		this.Resize( width, height );

		logtextview.Buffer.Text = "Log: ";
	
		// Create and display a fileChooserDialog
		FileChooserDialog chooser = new FileChooserDialog(
			"Please select a hex file created with Keil ...",
			this,
			FileChooserAction.Open,
			"Cancel", ResponseType.Cancel,
			"Open", ResponseType.Accept );
		chooser.AddFilter(hex_files_filter);
		if( chooser.Run() == ( int )ResponseType.Accept )
		{
			user_log ("Arquivo selecionado: " + chooser.Filename.ToString ());
			// Open the file for reading.
			System.IO.StreamReader file =
				System.IO.File.OpenText( chooser.Filename );

			string str_codigo_hex= file.ReadToEnd();
			user_log ("Lido:\n" + str_codigo_hex);
			codigo_atual.carregar_arquivo (str_codigo_hex);
			user_log ("Arquivo lido com sucesso.");
			txt_codigo.Buffer.Clear ();
			show_code (codigo_atual);
			export_vector (codigo_atual);

			// Set the MainWindow Title to the filename.
			this.Title = "Gravador do nrf24le1 -- " + chooser.Filename.ToString();
			file.Close(); // Close the file so as to not leave a mess.
		} // end if
		chooser.Destroy();
	}

	protected void OnCloseActionActivated (object sender, EventArgs e)
	{
		// Reset the logTreeView and change the window back to original size
		int width, height;
		this.GetDefaultSize( out width, out height );
		this.Resize( width, height );

		logtextview.Buffer.Text = "Log: ";
		txt_codigo.Buffer.Text = "";
		codigo_lido = new program_data ();
		codigo_atual = new program_data ();

		// Change the MainWindow Title back to the default.
		this.Title = "Gravador do nrf24le1";

	}

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		cbPortasSeriais.Clear();
		CellRendererText cell = new CellRendererText();
		cbPortasSeriais.PackStart(cell, false);
		cbPortasSeriais.AddAttribute(cell, "text", 0);
		ListStore store = new ListStore(typeof (string));
		cbPortasSeriais.Model = store;
		listar ();
	}

	protected void OnAboutActionActivated (object sender, EventArgs e)
	{
		// Create a new About dialog
		AboutDialog about = new AboutDialog();

		// Change the Dialog's properties to the appropriate values.
		about.ProgramName = "Gravador do nrf24le1";
		about.Version = "1.2";

		// Show the Dialog and pass it control
		about.Run();

		// Destroy the dialog
		about.Destroy();
	}
		
	protected void OnHelpActionActivated (object sender, EventArgs e)
	{
		System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
		try
		{
			// true is the default, but it is important not to set it to false
			myProcess.StartInfo.UseShellExecute = true;
			myProcess.StartInfo.FileName = "http://www.github.com/italogfernandes/ltad-propinsitu";
			myProcess.Start();
		}
		catch (Exception exc)
		{
			Console.WriteLine(exc.Message);
		}
	}

	protected void OnDialogInfoActionActivated (object sender, EventArgs e)
	{
		//txt_codigo.Buffer.Text = "Aqui é Body Builder Ipsum PORRA! Boraaa, Hora do Show Porra. Eu quero esse 13 daqui a pouquinho aí. AHHHHHHHHHHHHHHHHHHHHHH..., porra! Vem porra! Tá comigo porra. Vamo monstro!\n\nBoraaa, Hora do Show Porra. Ó o homem ali porra!, é 13 porra! Tá comigo porra. Vo derrubar tudo essas árvore do parque ibirapuera. É 13 porra! Ele tá olhando pra gente.\n\nÓ o homem ali porra!, é 13 porra! Aqui nóis constrói fibra, não é água com músculo. Ajuda o maluco que tá doente. AHHHHHHHHHHHHHHHHHHHHHH..., porra! Eita porra!, tá saindo da jaula o monstro! Negativa Bambam negativa.\n\nBoraaa, Hora do Show Porra. Birl! Tá comigo porra. Aqui é bodybuilder porra! Sai filho da puta! Vamo monstro!\n\nAqui é bodybuilder porra! Ó o homem ali porra!, é 13 porra! Vem porra! Tá comigo porra. Que não vai dá rapaiz, não vai dá essa porra. Sai de casa comi pra caralho porra.".Replace (". ", ".\n");
		txt_codigo.Buffer.Text = 
			"Software desenvolvido para realizar o upload de codigos" +
			"\n do tipo .hex para o transponder nrf24le1." +
			"\n Foi primeiramente desenvolvido para ser utilizado " +
			"\n junto ao arduino due configurado como um gravador." +
			"\n Tambem pode ser utilizado apenas gerando um vetor " +
			"\n em linguagem c contendo as informacoes do codigo .hex" +
			"\n carregado." +
			"\n No caso do PIC este vetor pode ser codiado e colocado nele." +
			"\n" +
			"\n" +
			"\n Desenvolvido por Italo Fernandes em 2016\n" +
			"\n GitHub: github.com/italogfernandes" +
			"\n Contato: italogsfernandes@gmail.com";
	}
		
	protected void OnBtnReadCodeClicked (object sender, EventArgs e)
	{
		readFlashCode ();
	}

	protected void OnBtnAutoBurnClicked (object sender, EventArgs e)
	{
		sendAutomaticModeSignal ();
	}
	protected void OnSaveActionActivated (object sender, EventArgs e)
	{
		// Create and display a fileChooserDialog
		FileChooserDialog chooser = new FileChooserDialog(
			"Please save your result ...",
			this,
			FileChooserAction.Save,
			"Cancel", ResponseType.Cancel,
			"Salvar", ResponseType.Accept );
		chooser.AddFilter(log_files_filter);

		if( chooser.Run() == ( int )ResponseType.Accept )
		{
			lbl_status2.Text = ("Arquivo selecionado: " + chooser.Filename.ToString ()+".burnerlog");
			// Open the file for writing.
			System.IO.StreamWriter file =
				System.IO.File.CreateText (chooser.Filename + ".burnerlog");
			file.Write("Dados da folha de código: \n");
			file.Write(txt_codigo.Buffer.Text);
			file.Write("Dados da folha de log: \n");
			file.Write(logtextview.Buffer.Text);
			lbl_status2.Text = "Arquivo salvo";
			// Set the MainWindow Title to the filename.
			file.Close(); // Close the file so as to not leave a mess.*/
		} // end if
		chooser.Destroy();
	}
}
