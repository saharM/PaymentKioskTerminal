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
	/// analizar los indicadores de largo de los valores formateados de los
	/// componentes de mensajería.
	/// </summary>
	public interface ILengthEncoder {

		/// <summary>
		/// Retorna el largo que ocupa el indicador de largo.
		/// </summary>
		int EncodedLength {

			get;
		}

		/// <summary>
		/// Formatea el largo de los datos del componente de mensajería.
		/// </summary>
		/// <param name="length">
		/// Es el largo de los datos del componente de mensajería.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo donde se almacenará la
		/// información formateada.
		/// </param>
		void Encode( int length, ref FormatterContext formatterContext);

		/// <summary>
		/// Convierte los datos formateados del indicador de largo
		/// de los datos del componente de mensajería.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes donde
		/// reside la información a decodificar.
		/// </param>
		/// <returns>
		/// Es el largo de los datos del componente de mensajería.
		/// </returns>
		int Decode( ref ParserContext parserContext);
	}
}
