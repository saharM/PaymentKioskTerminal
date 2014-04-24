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
	/// Esta interfaz define qué debe implementar una clase para formatear y
	/// analizar componentes de mensajería hacia y desde su forma cruda cuando
	/// son recibido y/o enviados hacia otro sistema.
	/// Formatea, analiza y produce datos del tipo <see langref="string"/>.
	/// </summary>
	public interface IStringEncoder {

		/// <summary>
		/// Calcula el largo de los datos formateados del componente de mensajería.
		/// </summary>
		/// <param name="dataLength">Es el largo de los datos del componente de
		/// mensajería.</param>
		/// <returns>Retorna el largo de los datos formateados.</returns>
		int GetEncodedLength( int dataLength);

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
		void Encode( string data, ref FormatterContext formatterContext);

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
		string Decode( ref ParserContext parserContext, int length);
	}
}
