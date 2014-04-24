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
using System.Net;

// TODO: Translate spanish -> english.

namespace Fanap.Utilities {

	/// <summary>
	/// Network utilities.
	/// </summary>
	public sealed class NetUtilities {

		#region Constructors
		/// <remarks>
		/// It prevents the <see cref="NetUtilities" /> instantiation, the methods and
        /// functions of this class are static.
		/// </remarks>
		private NetUtilities() {

		}
		#endregion

		#region Methods
		/// <summary>
        /// It verifies that the number of port indicated is a valid TCP port. 
		/// </summary>
		/// <param name="port">
		/// It's the port number to validate.
		/// </param>
		/// <returns>
		/// true if the port is valid, otherwise false.
		/// </returns>
		public static bool IsValidTcpPort( int port) {

			if ( port >= IPEndPoint.MinPort) {
				return ( port <= IPEndPoint.MaxPort);
			}

			return false;
		}
		#endregion
	}
}
