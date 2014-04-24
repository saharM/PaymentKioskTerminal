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
	/// Esta clase define la interfaz que debe implementar una clase
	/// para formatear el valor de un campo desde y hacia un valor de
	/// un tipo dado.
	/// </summary>
	/// <remarks>
	/// Las clases que implementen esta interfaz pueden opcionalmente
	/// tomar como argumentos, el formato a emplear.
	/// </remarks>
	public interface IStringFieldValueFormatter {

		/// <summary>
		/// Convierte el valor dado en un string.
		/// </summary>
		/// <param name="value">
		/// Es el valor a convertir.
		/// </param>
		/// <returns>
		/// Un string que representa al valor dado.
		/// </returns>
		string Format( object value);

		/// <summary>
		/// Convierte un valor de tipo string en un objeto del tipo dado.
		/// </summary>
		/// <param name="convertType">
		/// Es un objeto que representa el tipo hacia el que se desea
		/// convertir el valor dado.
		/// </param>
		/// <param name="value">
		/// Es el valor a convertir.
		/// </param>
		/// <returns>
		/// Un objeto del tipo deseado conteniendo el valor dado.
		/// </returns>
		object Parse( Type convertType, string value);
	}
}
