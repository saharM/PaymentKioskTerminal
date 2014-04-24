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

namespace Fanap.Messaging.ConditionalFormatting {

	/// <summary>
	/// This class implements the equals operator of two expressions.
	/// </summary>
	[Serializable]
	public class FieldValueEqualsBinaryOperator : EqualityEqualsOperator {

		private BinaryConstantExpression _valueExpression;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		public FieldValueEqualsBinaryOperator()
            : base() {

			_valueExpression = null;
		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="messageExpression">
		/// The message expression, source of the field value of the equality
        /// operator (left part of the operator).
		/// </param>
		/// <param name="valueExpression">
		/// The value expression of the equality operator (right part of the operator).
		/// </param>
		public FieldValueEqualsBinaryOperator( IMessageExpression messageExpression,
			BinaryConstantExpression valueExpression ) :
			base ( messageExpression, valueExpression ) {

		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns or sets the value expression of the equality operator (right
		/// part of the operator).
		/// </summary>
		public override IValueExpression ValueExpression {

			get {
				
				return _valueExpression;
			}

			set {

				if ( ( value != null ) && !( value is BinaryConstantExpression ) ) {
					throw new ArgumentException( "A BinaryConstantExpression type was expected." );
				}

				_valueExpression = value as BinaryConstantExpression;
			}
		}
		#endregion

		#region Methods
        // TODO: Pasarla a Trx.Utils.
        public bool CompareByteArrays( byte[] data1, byte[] data2 ) {

            // If both are null, they're equal
            if ( data1 == null && data2 == null ) {
                return true;
            }

            // If either but not both are null, they're not equal
            if ( data1 == null || data2 == null ) {
                return false;
            }

            if ( data1.Length != data2.Length ) {
                return false;
            }

            for ( int i = data1.Length - 1; i >= 0; i-- ) {
                if ( data1[i] != data2[i] ) {
                    return false;
                }
            }

            return true;
        }

		/// <summary>
		/// Evaluates the expression when parsing a message.
		/// </summary>
		/// <param name="parserContext">
		/// It's the parser context.
		/// </param>
		/// <returns>
		/// A boolean value.
		/// </returns>
		public override bool EvaluateParse( ref ParserContext parserContext ) {

            return CompareByteArrays( MessageExpression.GetLeafFieldValueBytes( ref parserContext, null ),
                _valueExpression.GetValue() );
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
		public override bool EvaluateFormat( Field field, ref FormatterContext formatterContext ) {

            return CompareByteArrays( MessageExpression.GetLeafFieldValueBytes( ref formatterContext, null ),
                _valueExpression.GetValue() );
        }
		#endregion
	}
}