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
using System.Runtime;
using Fanap.Messaging.Iso8583;

namespace Fanap.Messaging.ConditionalFormatting {

    /// <summary>
    /// This class implements field presence.
    /// </summary>
    [Serializable]
    public class IsSetExpression : IBooleanExpression {

        private IMessageExpression _messageExpression;

        #region Constructors
        /// <summary>
        /// It initializes a new instance of the class.
        /// </summary>
        public IsSetExpression() {

            _messageExpression = null;
        }

        /// <summary>
        /// It initializes a new instance of the class.
        /// </summary>
        /// <param name="messageExpression">
        /// The expression which supply the message.
        /// </param>
        public IsSetExpression( IMessageExpression messageExpression ) {

            _messageExpression = messageExpression;
        }
        #endregion

        #region Properties
        /// <summary>
        /// It returns or sets the message expression.
        /// </summary>
        public IMessageExpression MessageExpression {

            get {

                return _messageExpression;
            }

            set {

                _messageExpression = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the expression when parsing a message.
        /// </summary>
        /// <param name="parserContext">
        /// It's the parser context.
        /// </param>
        /// <returns>
        /// A boolean value.
        /// </returns>
        public bool EvaluateParse( ref ParserContext parserContext ) {

            return _messageExpression.GetLeafMessage( ref parserContext,
                null ).Fields.Contains( _messageExpression.GetLeafFieldNumber() );
        }

        /// <summary>
        /// Evaluates the expression when formatting a message.
        /// </summary>
        /// <param name="field">
        /// It's the field to format.
        /// </param>
        /// <param name="formatterContext">
        /// It's the context of formatting to be used by the method.
        /// </param>
        /// <returns>
        /// A boolean value.
        /// </returns>
        public bool EvaluateFormat( Field field, ref FormatterContext formatterContext ) {

            return _messageExpression.GetLeafMessage( ref formatterContext,
                null ).Fields.Contains( _messageExpression.GetLeafFieldNumber() );
        }
        #endregion
    }
}