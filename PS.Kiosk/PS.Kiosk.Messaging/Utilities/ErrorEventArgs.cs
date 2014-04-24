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
    /// This class defines the arguments for events that notify an error.
	/// </summary>
	public class ErrorEventArgs : EventArgs {

		private Exception _exception;

		#region Constructors
		/// <summary>
		/// Creates and initializes a new instance of class <see cref="ErrorEventArgs"/>.
		/// </summary>
		/// <param name="exception">
        /// It is the exception that produced the error that has been received.
		/// </param>
		/// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> it's null.
		/// </exception>
		public ErrorEventArgs( Exception exception) {

			if ( exception == null) {
				throw new ArgumentNullException( "exception");
			}

			_exception = exception;
		}
		#endregion

		#region Properties
		/// <summary>
        /// It returns the exception that has produced the error.
		/// </summary>
		public Exception Exception {

			get {

				return _exception;
			}
		}
		#endregion
	}
}
