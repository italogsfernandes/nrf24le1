using System;

namespace nrfBurner
{
	public class program_data
	{
		public byte[] hex_code = new byte[0x10];
		public string hex_str = "";
		//public byte [] [] hex_commands = new byte [300] [16];//SIZE,ADDR_H,ADDR_L,Type,Bytes,CHECK
		public program_data ()
		{
		}

		public void add_code(comando_cod_hex comando){
			int old_length = hex_code.Length;
			if (hex_code.Length < comando.addr + comando.qnt_bytes) {
				Array.Resize<byte> (ref hex_code,
					((comando.addr + comando.qnt_bytes + 16)/16)*16);//garantindo multiplo de 16, por conveniencia para mostrar
				for (int i = old_length; i < hex_code.Length; i++) {
					hex_code [i] = 0xFF;
				}
			}
			if (comando.data_type == 0x00) {
				for (int i = 0; i < comando.qnt_bytes; i++) {
					hex_code [comando.addr + i] = comando.data_bytes [i];
				}
			} else if (comando.data_type == 0x01) {
				Console.WriteLine ("A leitura chegou ao fim do arquivo.");
			} else {
				Console.WriteLine ("Erro inesperado com o tipo de arquivo.");
			}
		}
		public void carregar_arquivo(string arquivo_hex_str){
			this.hex_str = string.Copy (arquivo_hex_str);
			//Array.Resize<byte> (ref hex_code, 16);
			for (int i = 0; i < hex_code.Length; i++) {
				hex_code [i] = 0xFF;
			}
			foreach (var linha in arquivo_hex_str.Split ()) {
				if (linha.StartsWith (":", StringComparison.Ordinal)) {
					var comando_atual = new comando_cod_hex (linha);
					add_code (comando_atual);
					//add_hex_command (comando_atual.get_vector_hex ());
				}
			}
		}
	}
}