#region CopyrightExpression (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// CopyrightExpression © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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
using System.IO;

namespace Fanap.Messaging.ConditionalFormatting {

    /// <summary>
    /// This class implements a tokenizer to be used by SemanticParser.
    /// </summary>
	public class Tokenizer : yyInput {

		private LexicalAnalyzer _lexer;
		private Yytoken _currentToken = null;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="reader">
		/// It's the source of the expression to be parsed.
		/// </param>
		public Tokenizer( TextReader reader ) {

			if ( reader == null) {
				throw new ArgumentNullException( "reader");
			}

			_lexer = new LexicalAnalyzer( reader);
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns the index of the last parsed token.
		/// </summary>
		public int LastParsedTokenIndex {

			get {

				return _lexer.CurrentTokenIndex;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It parses the next token.
		/// </summary>
		/// <returns>
		/// true if the parse was performed, otherwise false.
		/// </returns>
		public bool advance() {

			_currentToken = _lexer.yylex();

			if ( _currentToken == null) {
				return false;
			} else {
				return true;
			}
		}

		/// <summary>
		/// It return the last parsed token.
		/// </summary>
		/// <returns>
		/// The last parsed token.
		/// </returns>
		public int token() {

			if ( _currentToken == null) {
				throw new InvalidOperationException();
			}

			return _currentToken.sym;
		}

		/// <summary>
		/// It returns the las parsed object.
		/// </summary>
		/// <returns>
		/// The las parsed object.
		/// </returns>
		public Object value() {

			if ( _currentToken == null) {
				throw new InvalidOperationException();
			}

			return _currentToken.value;
		}
		#endregion
	}
}
