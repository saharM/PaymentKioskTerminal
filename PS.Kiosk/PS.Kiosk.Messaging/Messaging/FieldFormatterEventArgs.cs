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
	/// Es el argumento para algunos eventos disparados por las colección
	/// de formateadores de mensajes.
	/// </summary>
	public class FieldFormatterEventArgs : EventArgs {

		private FieldFormatter _fieldFormatter;

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia de los argumentos a emplear
		/// en algunos eventos de las colecciones de formateadores de
		/// mensajes.
		/// </summary>
		/// <param name="fieldFormatter">
		/// Es el formateador de campo asociado al evento.
		/// </param>
		public FieldFormatterEventArgs( FieldFormatter fieldFormatter) {

			_fieldFormatter = fieldFormatter;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el formateador de campo asociado al evento.
		/// </summary>
		public FieldFormatter FieldFormatter {

			get {

				return _fieldFormatter;
			}
		}
		#endregion
	}
}
