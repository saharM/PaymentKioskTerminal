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
	/// This class implements a filler of values of type string.
	/// It performs the work adding or removing spaces at the
	/// end of the given value.
	/// </summary>
	/// <remarks>
	/// In addition, this filler verifies that the length of the data
	/// to fill up, does not exceed the expected length.
	/// </remarks>
	public sealed class SpacePaddingRight : StringPaddingRight {

		private static volatile SpacePaddingRight _instanceWithTruncate = null;
		private static volatile SpacePaddingRight _instanceWithoutTruncate = null;

		#region Constructors
		/// <summary>
		/// It constructs a new instance of the filler. It's private,
		/// in order to force the user to use <see cref="GetInstance"/>.
		/// </summary>
		/// <param name="truncate">
		/// <see langref="true"/> to discard data over the supported length,
		/// otherwise <see langref="false"/> to receive an exception if
		/// data doesn't fit in field.
		/// </param>
		private SpacePaddingRight( bool truncate) : base( truncate, ' ') {

		}
		#endregion

		#region Methods
		/// <summary>
		/// It returns an instance of class <see cref="SpacePaddingRight"/>.
		/// </summary>
		/// <param name="truncate">
		/// <see langref="true"/> to discard data over the supported length,
		/// otherwise <see langref="false"/> to receive an exception if
		/// data doesn't fit in field.
		/// </param>
		/// <returns>
		/// An instance of class <see cref="SpacePaddingRight"/>.
		/// </returns>
		public static SpacePaddingRight GetInstance( bool truncate) {

			SpacePaddingRight instance;

			if ( truncate) {
				instance = _instanceWithTruncate;
			} else {
				instance = _instanceWithoutTruncate;
			}

			if ( instance == null) {
				lock ( typeof( SpacePaddingRight)) {
					if ( instance == null) {
						instance = new SpacePaddingRight( truncate);

						if ( truncate) {
							_instanceWithTruncate = instance;
						} else {
							_instanceWithoutTruncate = instance;
						}
					}
				}
			}

			return instance;
		}
		#endregion
	}
}
