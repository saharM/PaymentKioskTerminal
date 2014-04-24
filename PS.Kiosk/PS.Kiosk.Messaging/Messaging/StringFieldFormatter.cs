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

// TODO: Translate spanish -> english.

using Fanap.Utilities;

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un formateador capaz de formatear o analizar un campo
	/// cuyos datos son cadenas de caracteres.
	/// </summary>
	public class StringFieldFormatter : FieldFormatter {

		private IStringEncoder _encoder;
		private IStringPadding _padding;
		private IStringValidator _validator;
		private IStringFieldValueFormatter _valueFormatter;
		private LengthManager _lengthManager;

		#region Constructors
		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder) : this( fieldNumber, lengthManager, encoder,
			null, null, null, string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="description">
		/// Es la descripción del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, string description) : this( fieldNumber,
			lengthManager, encoder, null, null, null, description) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding) :
			this( fieldNumber, lengthManager, encoder, padding, null, null,
			string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del campo.
		/// </param>
		/// <param name="description">
		/// Es la descripción del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding, string description) :
			this( fieldNumber, lengthManager, encoder, padding, null, null,
			description) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="validator">
		/// Es el objeto capaz de validar los datos del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringValidator validator) :
			this( fieldNumber, lengthManager, encoder, null, validator, null,
			string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="validator">
		/// Es el objeto capaz de validar los datos del campo.
		/// </param>
		/// <param name="description">
		/// Es la descripción del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringValidator validator, string description) :
			this( fieldNumber, lengthManager, encoder, null, validator, null,
			description) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del campo.
		/// </param>
		/// <param name="validator">
		/// Es el objeto capaz de validar los datos del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding, IStringValidator validator) :
			this( fieldNumber, lengthManager, encoder, padding, validator, null,
			string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del campo.
		/// </param>
		/// <param name="validator">
		/// Es el objeto capaz de validar los datos del campo.
		/// </param>
		/// <param name="valueFormatter">
		/// Es el objeto capaz de formatear/analizar los datos del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding, IStringValidator validator,
			IStringFieldValueFormatter valueFormatter) : this( fieldNumber,
			lengthManager, encoder, padding, validator, null, string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de campos de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz
		/// de formatear.
		/// </param>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del campo.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del campo.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del campo.
		/// </param>
		/// <param name="validator">
		/// Es el objeto capaz de validar los datos del campo.
		/// </param>
		/// <param name="valueFormatter">
		/// Es el objeto capaz de formatear/analizar los datos del campo.
		/// </param>
		/// <param name="description">
		/// Es la descripción del campo.
		/// </param>
		public StringFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding, IStringValidator validator,
			IStringFieldValueFormatter valueFormatter, string description) :
			base( fieldNumber, description) {

			if ( lengthManager == null) {
				throw new ArgumentNullException( "lengthManager");
			}

			if ( encoder == null) {
				throw new ArgumentNullException( "encoder");
			}

			_lengthManager = lengthManager;
			_encoder = encoder;
			_validator = validator;
			_valueFormatter = valueFormatter;

			if ( ( padding == null) && ( lengthManager is FixedLengthManager)) {
				_padding = SpacePaddingRight.GetInstance( false);
			} else {
				_padding = padding;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el administrador del largo de los datos del campo.
		/// </summary>
		public LengthManager LengthManager {

			get {

				return _lengthManager;
			}
		}

		/// <summary>
		/// Retorna el codificar/decodificar de los datos del campo.
		/// </summary>
		public IStringEncoder Encoder {

			get {

				return _encoder;
			}
		}

		/// <summary>
		/// Retorna el rellenador de los datos del campo.
		/// </summary>
		public IStringPadding Padding {

			get {

				return _padding;
			}
		}

		/// <summary>
		/// Retorna el validador de los datos del campo.
		/// </summary>
		public IStringValidator Validator {

			get {

				return _validator;
			}
		}

		/// <summary>
		/// Retorna el formateador/analizador de los datos del campo.
		/// </summary>
		public IStringFieldValueFormatter ValueFormatter {

			get {

				return _valueFormatter;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Formatea un campo cuyo valor es una cadena de caracteres.
		/// </summary>
		/// <param name="field">
		/// Es el campo a formatear.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo que debe ser empleado.
		/// </param>
		public override void Format( Field field,
			ref FormatterContext formatterContext) {

			if ( !( field is StringField)) {
				throw new ArgumentException( SR.StringMessageFieldExpected, "field");
                
			}

			string fieldValue = ( ( StringField)field).FieldValue;

			// Pad if padding available.
			if ( _padding != null) {
				fieldValue = _padding.Pad( fieldValue, _lengthManager.MaximumLength);
			}

			if ( _validator != null) {
				_validator.Validate( fieldValue);
			}

			if ( fieldValue == null) {
				_lengthManager.WriteLength( field, 0, 0, ref formatterContext);
				_lengthManager.WriteLengthTrailer( field, 0, 0, ref formatterContext);
			} else {
				_lengthManager.WriteLength( field, fieldValue.Length,
					_encoder.GetEncodedLength( fieldValue.Length),
					ref formatterContext);
				_encoder.Encode( fieldValue, ref formatterContext);
				_lengthManager.WriteLengthTrailer( field, fieldValue.Length,
					_encoder.GetEncodedLength( fieldValue.Length),
					ref formatterContext);
			}
		}

		/// <summary>
		/// Analiza la información contenida en un contexto de analisis y
		/// construcción de mensajes, y construye en base a ella un nuevo
		/// campo para el que el formateador se ha construido.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de analisis y construcción y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El nuevo campo contruido a partir de la información contenida en
		/// el contexto de análisis y construcción de mensajes.
		/// </returns>
		public override Field Parse( ref ParserContext parserContext) {

			// If MinValue, at this moment the length hasn't been decoded.
			if ( parserContext.DecodedLength == int.MinValue) {
				if ( !_lengthManager.EnoughData( ref parserContext)) {
					// Insufficient data to parse length, return null.
					return null;
				}

				// Save length in parser context just in case field value
				// can't be parsed at this time (more data needed).
				parserContext.DecodedLength =
					_lengthManager.ReadLength( ref parserContext);
			}

			if ( parserContext.DataLength < _encoder.GetEncodedLength(
				parserContext.DecodedLength)) {

				// Insufficient data to parse field value, return null.
				return null;
			}

			// Create the new messaging component with parsing context data.
			string fieldValue;
			if ( _padding == null) {
				fieldValue = _encoder.Decode( ref parserContext, parserContext.DecodedLength);
			} else {
				fieldValue = _padding.RemovePad( _encoder.Decode( ref parserContext,
					parserContext.DecodedLength));
			}

			if ( _validator != null) {
				_validator.Validate( fieldValue);
			}

			StringField field = new StringField( FieldNumber, fieldValue);

			_lengthManager.ReadLengthTrailer( ref parserContext);

			return field;
		}
		#endregion
	}
}