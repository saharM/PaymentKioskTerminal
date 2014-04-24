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
	/// It represents a string constant.
	/// </summary>
	[Serializable]
	public class StringConstantExpression : IValueExpression {

		private string _constant;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		public StringConstantExpression() {

			_constant = null;
		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="constant">
		/// It's the constant to store.
		/// </param>
		public StringConstantExpression( string constant ) {

			_constant = constant;
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
			}
		}
		#endregion
	}
}
