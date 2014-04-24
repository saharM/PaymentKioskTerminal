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

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa una clase capaz de formatear y analizar componentes de
	/// mensajería, utilizando los bytes en crudo como formato de datos.
	/// </summary>
	/// <remarks>
	/// This class implements the Singleton pattern, you must use
	/// <see cref="GetInstance"/> to acquire the instance.
	/// </remarks>
	public class BinaryEncoder : IBinaryEncoder {

		private static volatile BinaryEncoder _instance = null;

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia del codificador. Le damos el nivel
		/// del visibilidad 'private' para forzar al usuario a emplear
		/// <see cref="GetInstance"/>.
		/// </summary>
		private BinaryEncoder() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una instancia de la clase <see cref="BinaryEncoder"/>.
		/// </summary>
		/// <returns>
		/// Una instancia de la clase <see cref="BinaryEncoder"/>.
		/// </returns>
		public static BinaryEncoder GetInstance() {

			if ( _instance == null) {
				lock ( typeof( BinaryEncoder)) {
					if ( _instance == null) {
						_instance = new BinaryEncoder();
					}
				}
			}

			return _instance;
		}
		#endregion

		#region IBinaryEncoder Members
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

			return dataLength;
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
		public void Encode( byte[] data, ref FormatterContext formatterContext) {

			formatterContext.Write( data);
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
		public byte[] Decode( ref ParserContext parserContext, int length) {

			return parserContext.GetData( true, length);
		}
		#endregion
	}
}
