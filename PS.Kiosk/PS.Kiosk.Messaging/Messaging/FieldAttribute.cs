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
	/// Este atributo debe emplearse para asociar una propiedad de alguna
	/// clase, a un campo de un mensaje.
	/// </summary>
	/// <remarks>
	/// Los formateadores de mensajes deben emplear los formateadores de
	/// los valores de los campos, para formatear o analizar el valor
	/// del campo del mensaje.
	/// </remarks>
	[AttributeUsage( AttributeTargets.Property, AllowMultiple=false)]
	public sealed class FieldAttribute : Attribute {

		private int _fieldNumber;

		#region Constructors
		/// <summary>
		/// Inicializa una instancia de la clase <see cref="FieldAttribute" />.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo del mensaje al que se asocia la propiedad
		/// sobre la que se aplica el atributo.
		/// </param>
		public FieldAttribute( int fieldNumber) {

			_fieldNumber = fieldNumber;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el número de campo al que se asocia una propiedad.
		/// </summary>
		public int FieldNumber {

			get {

				return _fieldNumber;
			}
		}
		#endregion
	}
}
