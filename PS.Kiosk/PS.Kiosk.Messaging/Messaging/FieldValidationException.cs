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
using System.Runtime.Serialization;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Esta excepción representa un error que se produce cuando el valor
	/// de un campo no pasa la validación efectuada por alguna de las
	/// clases que implementan <see cref="IStringValidator"/>
	/// </summary>
	[Serializable()]
	public class StringValidationException : ApplicationException {

		#region Constructors
		/// <summary>
		/// Inicializa una nueva clase de tipo <see cref="StringValidationException" />.
		/// </summary>
		public StringValidationException() : base() {

		}

		/// <summary>
		/// Inicializa una nueva clase de tipo <see cref="StringValidationException" />.
		/// </summary>
		/// <param name="message">
		/// Es un mensaje a incluir con la excepción.
		/// </param>
		public StringValidationException( string message) : base( message) {

		}

		/// <summary>
		/// Inicializa una nueva clase de tipo <see cref="StringValidationException" />.
		/// </summary>
		/// <param name="message">
		/// Es un mensaje a incluir con la excepción.
		/// </param>
		/// <param name="innerException">
		/// Es la excepción que causó <see cref="StringValidationException" />.
		/// </param>
		public StringValidationException( string message, Exception innerException) :
			base( message, innerException) {

		}

		/// <summary>
		/// Inicializa una nueva clase de tipo <see cref="StringValidationException" />.
		/// </summary>
		/// <param name="info">
		/// Es un objeto de tipo <see cref="SerializationInfo" /> que almacena los datos
		/// serializados del objeto acerca de la excepción disparada.
		/// </param>
		/// <param name="context">
		/// Es un objeto de tipo <see cref="StreamingContext" /> que contiene la
		/// información de contexto acerca del origen o destino.
		/// </param>
		protected StringValidationException( SerializationInfo info,
			StreamingContext context) : base( info, context) {

		}
		#endregion
	}
}
