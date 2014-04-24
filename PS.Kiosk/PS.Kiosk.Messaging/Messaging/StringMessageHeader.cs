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
using System.Globalization;
using System.Text;
using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
    /// This class represents a string message header.
	/// </summary>
    [Serializable]
	public class StringMessageHeader : MessageHeader {

		private string _value;

		#region Constructors
		/// <summary>
		/// Contruye un nuevo cabezal de mensaje de tipo string.
		/// </summary>
		public StringMessageHeader() : base() {

			_value = null;
		}

		/// <summary>
		/// Contruye un nuevo cabezal de mensaje de tipo string.
		/// </summary>
		/// <param name="value">
		/// Es el valor del nuevo cabezal.
		/// </param>
		public StringMessageHeader( string value) : base() {

			_value = value;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna o asigna el valor del cabezal de tipo cadena de carateres.
		/// </summary>
		public string Value {

			get {

				return _value;
			}

			set {

				_value = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Convierte en una cadena de caracteres el valor del cabezal.
		/// </summary>
		/// <returns>
		/// Una cadena de caracteres que representan el valor del cabezal.
		/// </returns>
		public override string ToString() {

			if ( _value == null) {
				return string.Empty;
			} else {
				return _value;
			}
		}

		/// <summary>
		/// Convierte a un array de bytes el valor del cabezal.
		/// </summary>
		/// <returns>
		/// Un array de bytes.
		/// </returns>
		public override byte[] GetBytes() {

			if ( _value == null) {
				return null;
			}

            return FrameworkEncoding.GetInstance().Encoding.GetBytes( _value );
		}

		/// <summary>
		/// Construye una copia exacta del cabezal.
		/// </summary>
		/// <returns>
		/// Una copia exacta del cabezal.
		/// </returns>
		public override object Clone() {

			if ( _value == null) {
				return new StringMessageHeader();
			} else {
				return new StringMessageHeader( string.Copy( _value));
			}
		}

		/// <summary>
		/// Crea un nuevo cabezal de tipo string.
		/// </summary>
		/// <returns>
		/// Un nuevo cabezal de tipo string.
		/// </returns>
		public override MessagingComponent NewComponent() {

			return new StringMessageHeader();
		}
		#endregion
	}
}
