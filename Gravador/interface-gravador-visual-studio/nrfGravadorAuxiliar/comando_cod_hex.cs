using System;

namespace nrfGravadorAuxiliar
{
	public class comando_cod_hex
	{
		public byte addr_h = 0;
		public byte addr_l = 0;
		public byte qnt_bytes = 0;
		public byte data_type = 0;
		public byte[] data_bytes;
		public byte check_byte;
		public UInt16 addr = 0;
		public comando_cod_hex (string comando)
		{
			//Console.WriteLine (comando);
			qnt_bytes = Convert.ToByte (comando.Substring (1, 2), 16);
			addr_h = Convert.ToByte (comando.Substring (3, 2), 16);
			addr_l = Convert.ToByte (comando.Substring (5, 2), 16);
			data_type = Convert.ToByte (comando.Substring (7, 2), 16);
			data_bytes = new byte[qnt_bytes];
			for (int i = 0; i < qnt_bytes; i++) {
				data_bytes [i] = Convert.ToByte (
					comando.Substring (9 + i * 2, 2), 16);
			}
			check_byte =  Convert.ToByte (comando.Substring (9+qnt_bytes*2, 2), 16);

			addr = Convert.ToUInt16((addr_h << 8) | (addr_l));
		}
		public byte[] get_vector_hex(){
			byte[] vector_hex = new byte[qnt_bytes+5];
			vector_hex [0] = qnt_bytes;
			vector_hex [1] = addr_h;
			vector_hex [2] = addr_l;
			vector_hex [3] = data_type;
			for (int i = 4; i <qnt_bytes+4; i++) {
				vector_hex[i] = data_bytes[i-4];
			}
			vector_hex[qnt_bytes+4] = check_byte;
			return vector_hex;
		}
	}
}
	