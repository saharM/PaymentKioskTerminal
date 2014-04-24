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
using System.IO;
using Fanap.Utilities;

namespace Fanap.Messaging.ConditionalFormatting {

	/// <summary>
	/// Implements a field formatter, this class holds two field formatters
	/// which are used based on a conditional statement.
	/// </summary>
    /// <remarks>
    /// This class implements <see cref="IConditionalFieldEvaluator"/> as a facade
    /// when an expression is used to instantiate the class.
    /// </remarks>
	public class ConditionalFieldFormatter : FieldFormatter, IConditionalFieldEvaluator {

		private IConditionalFieldEvaluator _evaluator;
		private FieldFormatter _trueFormatter;
		private FieldFormatter _falseFormatter;
        private string _expression = null;
        private IBooleanExpression _compiledExpression = null;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="fieldNumber">
		/// It's the number of the field this formatter formats/parse.
		/// </param>
		/// <param name="expression">
		/// It's the expression to evaluate, based in the result <paramref name="trueFormatter"/>
		/// or <paramref name="falseFormatter" /> are used in the formatting/parsing of a message.
		/// </param>
		/// <param name="trueFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is true.
		/// </param>
		/// <param name="falseFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is false.
		/// </param>
		/// <param name="description">
		/// It's the description of the field formatter.
		/// </param>
		public ConditionalFieldFormatter( int fieldNumber, string expression,
			FieldFormatter trueFormatter, FieldFormatter falseFormatter,
			string description) : base( fieldNumber, description) {

			if ( trueFormatter == null ) {
				throw new ArgumentNullException( "trueFormatter" );
			}

			if ( falseFormatter == null ) {
				throw new ArgumentNullException( "falseFormatter" );
			}

			_expression = expression;
			_trueFormatter = trueFormatter;
			_falseFormatter = falseFormatter;

			Tokenizer tokenizer = new Tokenizer(
				new StringReader( _expression ) );
			SemanticParser sp = new SemanticParser();

			object result = null;

			try {
				result = sp.yyparse( tokenizer );
			} catch ( Exception ex ) {
				throw new ExpressionCompileException( ex.Message, tokenizer.LastParsedTokenIndex );
			}

			_compiledExpression = result as IBooleanExpression;

			if ( _compiledExpression == null ) {
				throw new ApplicationException( "Unknown result from expression." );
			}

			_evaluator = this;
		}
		
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="fieldNumber">
		/// It's the number of the field this formatter formats/parse.
		/// </param>
		/// <param name="expression">
		/// It's the expression to evaluate, based in the result <paramref name="trueFormatter"/>
		/// or <paramref name="falseFormatter" /> are used in the formatting/parsing of a message.
		/// </param>
		/// <param name="trueFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is true.
		/// </param>
		/// <param name="falseFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is false.
		/// </param>
		public ConditionalFieldFormatter( int fieldNumber, string expression,
			FieldFormatter trueFormatter, FieldFormatter falseFormatter) :
			this( fieldNumber, expression, trueFormatter, falseFormatter, string.Empty ) {

		}

		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="fieldNumber">
		/// It's the number of the field this formatter formats/parse.
		/// </param>
		/// <param name="evaluator">
		/// It's the evaluator which decides which field formatter must be used between
		/// <paramref name="trueFormatter"/> and <paramref name="falseFormatter" />,
		/// in the formatting/parsing of a message.
		/// </param>
		/// <param name="trueFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is true.
		/// </param>
		/// <param name="falseFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is false.
		/// </param>
		/// <param name="description">
		/// It's the description of the field formatter.
		/// </param>
		public ConditionalFieldFormatter( int fieldNumber,
			IConditionalFieldEvaluator evaluator,
			FieldFormatter trueFormatter, FieldFormatter falseFormatter,
			string description) : base( fieldNumber, description) {

			if ( evaluator == null ) {
				throw new ArgumentNullException( "evaluator" );
			}

			if ( trueFormatter == null ) {
				throw new ArgumentNullException( "trueFormatter" );
			}

			if ( falseFormatter == null ) {
				throw new ArgumentNullException( "falseFormatter" );
			}

			_evaluator = evaluator;
			_trueFormatter = trueFormatter;
			_falseFormatter = falseFormatter;
		}
		
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="fieldNumber">
		/// It's the number of the field this formatter formats/parse.
		/// </param>
        /// <param name="evaluator">
        /// It's the evaluator which decides which field formatter must be used between
        /// <paramref name="trueFormatter"/> and <paramref name="falseFormatter" />,
        /// in the formatting/parsing of a message.
        /// </param>
		/// <param name="trueFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is true.
		/// </param>
		/// <param name="falseFormatter">
		/// It's the formatter to be used when <paramref name="expression"> is false.
		/// </param>
		public ConditionalFieldFormatter( int fieldNumber,
			IConditionalFieldEvaluator evaluator,
			FieldFormatter trueFormatter, FieldFormatter falseFormatter) :
			this( fieldNumber, evaluator, trueFormatter, falseFormatter, string.Empty ) {

		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns the expression evaluated to select a field formatter.
		/// </summary>
		public string Expression {

			get {

				return _expression;
			}
		}

        /// <summary>
        /// It returns the compiled expression which decides which field formatter must
        /// be used between <paramref name="trueFormatter"/> and <paramref name="falseFormatter" />,
        /// in the formatting/parsing of a message.
        /// </summary>
        public IBooleanExpression CompiledExpression {

            get {

                return _compiledExpression;
            }
        }

		/// <summary>
        /// It returns the evaluator which decides which field formatter must be used between
        /// <paramref name="trueFormatter"/> and <paramref name="falseFormatter" />,
        /// in the formatting/parsing of a message.
		/// </summary>
		public IConditionalFieldEvaluator Evaluator {

			get {

				return _evaluator;
			}
		}

		/// <summary>
		/// It returns the formatter used when the <see cref="Expression"/>
		/// evaluates to true.
		/// </summary>
		public FieldFormatter TrueFormatter {

			get {

				return _trueFormatter;
			}
		}

		/// <summary>
		/// It returns the formatter used when the <see cref="Expression"/>
		/// evaluates to false.
		/// </summary>
		public FieldFormatter FalseFormatter {

			get {

				return _falseFormatter;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Formats the specified field.
		/// </summary>
		/// <param name="field">
		/// It's the field to format.
		/// </param>
		/// <param name="formatterContext">
		/// It's the context of formatting to be used by the method.
		/// </param>
		public override void Format( Field field, ref FormatterContext formatterContext) {

			if ( _evaluator.EvaluateFormat( field, ref formatterContext ) ) {
				_trueFormatter.Format( field, ref formatterContext );
			}
			else {
				_falseFormatter.Format( field, ref formatterContext );
			}
		}

		/// <summary>
		/// It parses the information in the parser context and builds the field.
		/// </summary>
		/// <param name="parserContext">
		/// It's the parser context.
		/// </param>
		/// <returns>
		/// The new field built with the information found in the parser context.
		/// </returns>
		public override Field Parse( ref ParserContext parserContext) {

			if ( _evaluator.EvaluateParse( ref parserContext ) ) {
				return _trueFormatter.Parse( ref parserContext );
			}
			else {
				return _falseFormatter.Parse( ref parserContext );
			}
		}

		/// <summary>
		/// Evaluates the field to format to decide the field formatter to be used.
		/// </summary>
		/// <param name="field">
		/// It's the field to format.
		/// </param>
		/// <param name="formatterContext">
		/// It's the context of formatting to be used by the method.
		/// </param>
		/// <returns>
		/// A logical value indicating the field formatter to be used.
		/// </returns>
		public bool EvaluateFormat( Field field, ref FormatterContext formatterContext ) {

			if ( _compiledExpression == null ) {
				throw new ApplicationException(
					"The conditional field formatter does not know the expression to evaluate." );
			}

			return _compiledExpression.EvaluateFormat( field, ref formatterContext );
		}

		/// <summary>
		/// Evaluates the parser context to decide the field formatter to be used.
		/// </summary>
		/// <param name="parserContext">
		/// It's the parser context.
		/// </param>
		/// <returns>
		/// A logical value indicating the field formatter to be used.
		/// </returns>
		public bool EvaluateParse( ref ParserContext parserContext ) {

			if ( _compiledExpression == null ) {
				throw new ApplicationException(
					"The conditional field formatter does not know the expression to evaluate." );
			}

			return _compiledExpression.EvaluateParse( ref parserContext );
		}
		#endregion
	}
}
