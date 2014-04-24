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
	/// Define la interfaz que debe implementar una clase para
	/// poder ser empleada como formateador de cabezales de mensajes.
	/// </summary>
	public interface IMessageHeaderFormatter {

		/// <summary>
		/// Formatea un cabezal.
		/// </summary>
		/// <param name="header">
		/// Es el cabezal a formatear.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo que debe ser empleado.
		/// </param>
		void Format( MessageHeader header, ref FormatterContext formatterContext);

		/// <summary>
		/// Analiza la información contenida en un contexto de analisis y construcción
		/// de mensajes, y construye en base a ella un nuevo cabezal para el que el
		/// formateador se ha construido.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de analisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El nuevo cabezal contruido a partir de la información contenida en el
		/// contexto de análisis y construcción de mensajes.
		/// </returns>
		MessageHeader Parse( ref ParserContext parserContext);
	}
}
