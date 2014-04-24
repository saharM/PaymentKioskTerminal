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
using System.Runtime;

namespace Fanap.Messaging.ConditionalFormatting {

	/// <summary>
	/// It represents a binary string constant.
	/// </summary>
	[Serializable]
	public class BinaryConstantExpression : IValueExpression {

		private string _constant;
		private byte[] _value;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		public BinaryConstantExpression() {

			Constant = null;
		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="constant">
		/// It's the constant to store.
		/// </param>
		public BinaryConstantExpression( string constant ) {

			Constant = constant;
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns or sets the string constant.
		/// </summary>
		public string Constant {

			get {

				return _constant;
			}

			set {

				_constant = value;
				
				if ( _constant == null ) {
					_value = null;
				}
				else {
					_value = new byte[( _constant.Length + 1) >> 1];

					// Initialize result bytes.
					for ( int i = _value.Length - 1; i >= 0; i--) {
						_value[i] = 0;
					}

					// Format data.
					for ( int i = 0; i < _constant.Length; i++) {
                        if ( _constant[i] < 0x40 ) {
							_value[( i >> 1)] |= ( byte)
								( ( ( _constant[i]) - 0x30) << ( ( i & 1) == 1 ? 0 : 4));
						} else {
							_value[( i >> 1)] |= ( byte)
								( ( ( _constant[i]) - 0x37) << ( ( i & 1) == 1 ? 0 : 4));
						}
					}
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It returns the binary value represented by the constant.
		/// </summary>
		/// <returns>
		/// A byte array.
		/// </returns>
		public byte[] GetValue() {

			return _value;
		}
		#endregion
	}
}
