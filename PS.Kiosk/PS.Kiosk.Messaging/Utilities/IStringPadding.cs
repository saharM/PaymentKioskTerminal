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

namespace Fanap.Utilities {

	/// <summary>
	/// It defines what must implement a class to pad a value of type string.
	/// </summary>
	public interface IStringPadding {

		/// <summary>
		/// It performs the pad of a value of type string.
		/// </summary>
		/// <param name="data">
		/// It is the value that should be padded.
		/// </param>
		/// <param name="totalWidth">
		/// Is the maximum length of the resultant string.
		/// </param>
		/// <returns>
		/// A string with the padded value.
		/// </returns>
		string Pad( string data, int totalWidth);

		/// <summary>
		/// It eliminates the padding in the given value.
		/// </summary>
		/// <param name="data">
		/// The padded data.
		/// </param>
		/// <returns>
		/// The value without the padding.
		/// </returns>
		string RemovePad( string data);
	}
}
