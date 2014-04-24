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

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa un identificador de mensaje básico que para
	/// realizar su función concatena los campos indicados en el
	/// momento de instanciación.
	/// </summary>
	public class BasicMessagesIdentifier : IMessagesIdentifier {

		private int[] _fields;

		#region Constructors
		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="fields">
		/// Son los campos a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int[] fields) {

			_fields = fields;
		}

		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="firstFieldNumber">
		/// Es el primer campo a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		/// <param name="secondFieldNumber">
		/// Es el segundo campo a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int firstFieldNumber,
			int secondFieldNumber) {

			_fields = new int[] { firstFieldNumber, secondFieldNumber};
		}

		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el campo que contiene el identificador del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int fieldNumber) {

			_fields = new int[] { fieldNumber};
		}
		#endregion

		#region Methods
		/// <summary>
		/// Calcula el identificador del mensaje dado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje del que se quiere saber su identificador.
		/// </param>
		/// <returns>
		/// El identificador del mensaje.
		/// </returns>
		public object ComputeIdentifier( Message message) {

			if ( !message.Fields.Contains( _fields)) {
				return null;
			}

			if ( _fields.Length > 1) {
				StringBuilder identifier = new StringBuilder();

				for ( int i = 0; i < _fields.Length; i++) {
					identifier.Append( message.Fields[_fields[i]].ToString());
				}

				return identifier.ToString();
			} else {
				return message.Fields[_fields[0]].ToString();
			}
		}
		#endregion
	}
}
