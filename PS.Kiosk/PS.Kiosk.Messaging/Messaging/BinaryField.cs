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
using log4net.ObjectRenderer;
using Fanap.Utilities;

namespace Fanap.Messaging {

	/// <summary>
    /// Implements a binary message field component.
	/// </summary>
    [Serializable]
	public class BinaryField : Field {

		private byte[] _value;

		#region Constructors
		/// <summary>
        /// It initializes a new binary message field component.
		/// </summary>
		/// <param name="fieldNumber">
        /// It's the field number of the new field.
        /// </param>
		public BinaryField( int fieldNumber) : base( fieldNumber) {

			_value = null;
		}

		/// <summary>
        /// It initializes a new binary message field component.
		/// </summary>
		/// <param name="fieldNumber">
        /// It's the field number of the new field.
        /// </param>
		/// <param name="value">
        /// It's the value of the new field.
        /// </param>
		public BinaryField( int fieldNumber, byte[] value) : base( fieldNumber) {

			_value = value;
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns or sets the value of the field.
		/// </summary>
		public override object Value {

			get {

				return _value;
			}

			set {

				if ( value is Byte[]) {
					_value = ( byte[])value;
				} else if ( value == null) {
					_value = null;
				} else if ( value is string) {
                    _value = FrameworkEncoding.GetInstance().Encoding.GetBytes( ( string )value );
				} else {
					throw new ArgumentException( SR.CantHandleParameterType, "value");
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
        /// It sets the value of the field.
		/// </summary>
		public void SetFieldValue( string value) {

			_value = FrameworkEncoding.GetInstance().Encoding.GetBytes( ( string )value );
		}

		/// <summary>
        /// It sets the value of the field.
        /// </summary>
		public void SetFieldValue( byte[] value) {

			_value = value;
		}

		/// <summary>
        /// It returns a string representation of the field value.
		/// </summary>
		/// <returns>
        /// A string representing the field value.
		/// </returns>
        /// <remarks>
        /// If the value is null, this function returns an empty string.
        /// </remarks>
		public override string ToString() {

			if ( _value == null) {
				return string.Empty;
			} else {
				return FrameworkEncoding.GetInstance().Encoding.GetString( _value );
			}
		}

		/// <summary>
        /// It returns the field value.
		/// </summary>
		/// <returns>
        /// An array of bytes, or null if the field value is null.
        /// </returns>
		public override byte[] GetBytes() {

			return _value;
		}

		/// <summary>
        /// Clones the field.
		/// </summary>
		/// <returns>
		/// A clone of the field instance.
		/// </returns>
		public override object Clone() {

			byte[] clonedValue = null;

			if ( _value != null) {
				clonedValue = new byte[_value.Length];
				_value.CopyTo( clonedValue, 0);
			}

			return new BinaryField( FieldNumber, clonedValue);
		}

		/// <summary>
        /// It creates a new binary field.
		/// </summary>
		/// <returns>
        /// A new binary field.
        /// </returns>
		public override MessagingComponent NewComponent() {

			return new BinaryField( FieldNumber);
		}
		#endregion
	}
}
