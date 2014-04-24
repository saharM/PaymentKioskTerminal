#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;
using System.Text;
using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa una clase capaz de formatear y analizar componentes de
	/// mensajería, utilizando cadenas de caracteres como formato de datos.
	/// Cada caracter de la información a formatear es tomado en su representación
	/// hexadecimal y almacenado en dos bytes, cada uno guardando el
	/// correspondiente dígito hexadecimal de la información. A modo de ejemplo
	/// si un caracter contiene el valor decimal '58', su representación hexadecimal
	/// es '3A', cuando el codificador formatee esta información producirá
	/// dos caracteres, el primero con un '3' (valor decimal 51, hexadecimal
	/// 33), y el segundo con una 'A' (valor decimal 65, hexadecimal 41).
	/// La información producida por esta clase siempre contendrá datos
	/// de tipo ASCII sin caracteres de control.
	/// </summary>
	/// <remarks>
	/// This class implements the Singleton pattern, you must use
	/// <see cref="GetInstance"/> to acquire the instance.
	/// </remarks>
	public class HexadecimalStringEncoder : IStringEncoder {

		private static volatile HexadecimalStringEncoder _instance = null;

		private static byte[] HexadecimalAsciiDigits = {
			0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46};

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia del codificador. Le damos el nivel
		/// del visibilidad 'private' para forzar al usuario a emplear
		/// <see cref="GetInstance"/>.
		/// </summary>
		private HexadecimalStringEncoder() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una instancia de la clase <see cref="HexadecimalStringEncoder"/>.
		/// </summary>
		/// <returns>
		/// Una instancia de la clase <see cref="HexadecimalStringEncoder"/>.
		/// </returns>
		public static HexadecimalStringEncoder GetInstance() {

			if ( _instance == null) {
				lock ( typeof( HexadecimalStringEncoder)) {
					if ( _instance == null) {
						_instance = new HexadecimalStringEncoder();
					}
				}
			}

			return _instance;
		}
		#endregion

		#region IStringEncoder Members
		/// <summary>
		/// Calcula el largo de los datos formateados del componente de mensajería.
		/// </summary>
		/// <param name="dataLength">
		/// Es el largo de los datos del componente de mensajería.
		/// </param>
		/// <returns>
		/// Retorna el largo de los datos formateados.
		/// </returns>
		public int GetEncodedLength( int dataLength) {

			return dataLength << 1;
		}

		/// <summary>
		/// Formatea los datos del componente de mensajería.
		/// </summary>
		/// <param name="data">
		/// Son los datos del componente de mensajería.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo donde se almacenará la
		/// información formateada.
		/// </param>
		public void Encode( string data, ref FormatterContext formatterContext) {

			// Check if we must resize formatter context buffer.
			if ( formatterContext.FreeBufferSpace < ( data.Length << 1)) {
				formatterContext.ResizeBuffer( data.Length << 1);
			}

			byte[] buffer = formatterContext.GetBuffer();
			int offset = formatterContext.UpperDataBound;

			// Format data.
			for ( int i = 0; i < data.Length; i++) {
				buffer[offset + ( i << 1)] = HexadecimalAsciiDigits[ ( ( ( byte)data[i]) & 0xF0) >> 4];
				buffer[offset + ( i << 1) + 1] = HexadecimalAsciiDigits[( ( byte)data[i]) & 0x0F];
			}

			formatterContext.UpperDataBound += data.Length << 1;
		}

		/// <summary>
		/// Convierte los datos formateados en datos válidos del componente
		/// de mensajería.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes donde
		/// reside la información a decodificar.
		/// </param>
		/// <param name="length">
		/// Es la cantidad de información que se desea obtener.
		/// </param>
		/// <returns>
		/// Una cadena de caracteres con los datos del componente de mensajería.
		/// </returns>
		public string Decode( ref ParserContext parserContext, int length) {

			if ( parserContext.DataLength < ( length << 1)) {
				throw new ArgumentException( SR.InsufficientData, "length");
			}

			byte[] result = new byte[length];
			byte[] buffer = parserContext.GetBuffer();
			int offset = parserContext.LowerDataBound;

			for ( int i = offset + (length << 1) - 1; i >= offset; i--) {

				int right = buffer[i] > 0x40 ? 10 + ( buffer[i] > 0x60 ?
					buffer[i] - 0x61 : buffer[i] - 0x41) : buffer[i] - 0x30;
				int left = buffer[--i] > 0x40 ? 10 + ( buffer[i] > 0x60 ?
					buffer[i] - 0x61 : buffer[i] - 0x41) : buffer[i] - 0x30;

				result[(i - offset) >> 1] = ( byte)( ( left << 4) | right);
			}

			parserContext.Consumed( length << 1);

            return FrameworkEncoding.GetInstance().Encoding.GetString( result );
		}
		#endregion
	}
}