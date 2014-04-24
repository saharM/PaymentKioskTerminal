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

namespace Fanap.Messaging.ConditionalFormatting {

	/// <summary>
    /// This class implements the exception raised when a expression
	/// parser locates an error.
	/// </summary>
	[Serializable]
	public class ExpressionCompileException : ApplicationException {

		private int _lastParsedTokenIndex = -1;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionCompileException" /> 
		/// class.
		/// </summary>
		public ExpressionCompileException() : base() {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionCompileException" /> 
		/// class with a descriptive message.
		/// </summary>
		/// <param name="message">
		/// A descriptive message to include with the exception.
		/// </param>
		public ExpressionCompileException( string message ) : base( message ) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionCompileException" /> 
		/// class with a descriptive message.
		/// </summary>
		/// <param name="message">
		/// A descriptive message to include with the exception.
		/// </param>
		/// <param name="lastParsedTokenIndex">
		/// The index of the last parsed token.
		/// </param>
		public ExpressionCompileException( string message, int lastParsedTokenIndex )
			: base( message ) {

			_lastParsedTokenIndex = lastParsedTokenIndex;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionCompileException" /> 
		/// class with the specified descriptive message and inner exception.
		/// </summary>
		/// <param name="message">
		/// A descriptive message to include with the exception.
		/// </param>
		/// <param name="innerException">
		/// A nested exception that is the cause of the current exception.
		/// </param>
		public ExpressionCompileException( string message,
			Exception innerException ) : base( message, innerException ) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionCompileException" /> 
		/// class with serialized data.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo" /> that holds the serialized object data
		/// about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="StreamingContext" /> that contains contextual information
		/// about the source or destination.
		/// </param>
		protected ExpressionCompileException( SerializationInfo info,
			StreamingContext context) : base( info, context ) {

		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns or sets the index of the last parsed token.
		/// </summary>
		public int LastParsedTokenIndex {

			get {

				return _lastParsedTokenIndex;
			}

			set {

				_lastParsedTokenIndex = value;
			}
		}
		#endregion
	}
}