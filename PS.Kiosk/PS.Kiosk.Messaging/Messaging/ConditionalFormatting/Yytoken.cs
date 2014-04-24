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

namespace Fanap.Messaging.ConditionalFormatting {

	/// <summary>
	/// It represents a token in an expression.
	/// </summary>
	public class Yytoken {

		public int sym;
		public int left;
		public int right;
		public object value;
		private int _position;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="sym_num">
		/// The number of the token.
		/// </param>
		/// <param name="position">
		/// The start position of the token in the expression.
		/// </param>
		public Yytoken( int sym_num, int position) :
			this( sym_num, position, -1, -1, null) {

		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="sym_num">
		/// The number of the token.
		/// </param>
		/// <param name="position">
		/// The start position of the token in the expression.
		/// </param>
		/// <param name="l">
		/// The left token.
		/// </param>
		/// <param name="r">
		/// The right token.
		/// </param>
		public Yytoken( int sym_num, int position, int l, int r) :
			this( sym_num, position, l, r, null) {

		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="sym_num">
		/// The number of the token.
		/// </param>
		/// <param name="position">
		/// The start position of the token in the expression.
		/// </param>
		/// <param name="o">
		/// The value of the token.
		/// </param>
		public Yytoken( int sym_num, int position, object o) :
			this( sym_num, position, -1, -1, o) {

		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="sym_num">
		/// The number of the token.
		/// </param>
		/// <param name="position">
		/// The start position of the token in the expression.
		/// </param>
		/// <param name="l">
		/// The left token.
		/// </param>
		/// <param name="r">
		/// The right token.
		/// </param>
		/// <param name="o">
		/// The value of the token.
		/// </param>
		public Yytoken( int sym_num, int position, int l, int r, object o) {

			sym = sym_num;
			left = l;
			right = r;
			value = o;
			_position = position;
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns the start position in the expression of the token.
		/// </summary>
		public int Position {

			get {

				return _position;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It returns a string representation of the token.
		/// </summary>
		/// <returns>
		/// A string representation of the token.
		/// </returns>
		public override string ToString() {
			
			return "#"+sym;
		}
		#endregion
	}
}
