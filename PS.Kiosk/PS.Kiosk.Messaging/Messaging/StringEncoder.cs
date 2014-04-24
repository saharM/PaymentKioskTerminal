#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright � 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa una clase capaz de formatear y analizar componentes de
	/// mensajer�a, utilizando como formato de datos el set de caracteres
	/// ASCII.
	/// </summary>
	/// <remarks>
	/// This class implements the Singleton pattern, you must use
	/// <see cref="GetInstance"/> to acquire the instance.
	/// </remarks>
	public class StringEncoder : IStringEncoder {

		private static volatile StringEncoder _instance = null;

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia del codificador. Le damos el nivel
		/// del visibilidad 'private' para forzar al usuario a emplear
		/// <see cref="GetInstance"/>.
		/// </summary>
		private StringEncoder() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una instancia de la clase <see cref="StringEncoder"/>.
		/// </summary>
		/// <returns>
		/// Una instancia de la clase <see cref="StringEncoder"/>.
		/// </returns>
		public static StringEncoder GetInstance() {

			if ( _instance == null) {
				lock ( typeof( StringEncoder)) {
					if ( _instance == null) {
						_instance = new StringEncoder();
					}
				}
			}

			return _instance;
		}
		#endregion

		#region IStringEncoder Members
		/// <summary>
		/// Calcula el largo de los datos formateados del componente de mensajer�a.
		/// </summary>
		/// <param name="dataLength">
		/// Es el largo de los datos del componente de mensajer�a.
		/// </param>
		/// <returns>
		/// Retorna el largo de los datos formateados.
		/// </returns>
		public int GetEncodedLength( int dataLength) {

			return dataLength;
		}

		/// <summary>
		/// Formatea los datos del componente de mensajer�a.
		/// </summary>
		/// <param name="data">
		/// Son los datos del componente de mensajer�a.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo donde se almacenar� la
		/// informaci�n formateada.
		/// </param>
		public void Encode( string data, ref FormatterContext formatterContext) {

			formatterContext.Write( data);
		}

		/// <summary>
		/// Convierte los datos formateados en datos v�lidos del componente
		/// de mensajer�a.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de an�lisis y construcci�n de mensajes donde
		/// reside la informaci�n a decodificar.
		/// </param>
		/// <param name="length">
		/// Es la cantidad de informaci�n que se desea obtener.
		/// </param>
		/// <returns>
		/// Una cadena de caracteres con los datos del componente de mensajer�a.
		/// </returns>
		public string Decode( ref ParserContext parserContext, int length) {

			return parserContext.GetDataAsString( true, length);
		}
		#endregion
	}
}
