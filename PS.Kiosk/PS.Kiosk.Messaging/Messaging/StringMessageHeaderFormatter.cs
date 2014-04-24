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

using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un formateador capaz de formatear o analizar un cabezales
	/// cuyos datos son cadenas de caracteres.
	/// </summary>
	public class StringMessageHeaderFormatter : IMessageHeaderFormatter {

		private IStringEncoder _encoder;
		private IStringPadding _padding;
		private LengthManager _lengthManager;

		#region Constructors
		/// <summary>
		/// Construye un nuevo formateador de cabezales de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del cabezal.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del cabezal.
		/// </param>
		public StringMessageHeaderFormatter( LengthManager lengthManager,
			IStringEncoder encoder) :
			this( lengthManager, encoder, null) {

		}

		/// <summary>
		/// Construye un nuevo formateador de cabezales de mensajes cuyos
		/// datos son de tipo cadena de caracteres.
		/// </summary>
		/// <param name="lengthManager">
		/// Es el objeto que administra el largo de los datos del cabezal.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del cabezal.
		/// </param>
		/// <param name="padding">
		/// Es el objeto capaz de rellenar los datos del cabezal.
		/// </param>
		public StringMessageHeaderFormatter( LengthManager lengthManager,
			IStringEncoder encoder, IStringPadding padding) : base() {

			if ( lengthManager == null) {
				throw new ArgumentNullException( "lengthManager");
			}

			if ( encoder == null) {
				throw new ArgumentNullException( "encoder");
			}

			_lengthManager = lengthManager;
			_encoder = encoder;

			if ( ( padding == null) && ( lengthManager is FixedLengthManager)) {
				_padding = SpacePaddingRight.GetInstance( false);
			} else {
				_padding = padding;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el administrador del largo de los datos del cabezal.
		/// </summary>
		public LengthManager LengthManager {

			get {

				return _lengthManager;
			}
		}

		/// <summary>
		/// Retorna el codificar/decodificar de los datos del cabezal.
		/// </summary>
		public IStringEncoder Encoder {

			get {

				return _encoder;
			}
		}

		/// <summary>
		/// Retorna el rellenador de los datos del cabezal.
		/// </summary>
		public IStringPadding Padding {

			get {

				return _padding;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Formatea un campo cuyo valor es una cadena de caracteres.
		/// </summary>
		/// <param name="header">
		/// Es el cabezal a formatear.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo que debe ser empleado.
		/// </param>
		public virtual void Format( MessageHeader header,
			ref FormatterContext formatterContext) {

			string headerValue = null;

			if ( header != null) {
				if ( !( header is StringMessageHeader)) {
					throw new ArgumentException( SR.StringHeaderExpected, "header");
				}

				headerValue = ( ( StringMessageHeader)header).Value;
			}

			// Pad if padding available.
			if ( _padding != null) {
				headerValue = _padding.Pad( headerValue,
					_lengthManager.MaximumLength);
			}

			if ( headerValue == null) {
				_lengthManager.WriteLength( header, 0, 0, ref formatterContext);
				_lengthManager.WriteLengthTrailer( header, 0, 0, ref formatterContext);
			} else {
				_lengthManager.WriteLength( header, headerValue.Length,
					_encoder.GetEncodedLength( headerValue.Length),
					ref formatterContext);
				_encoder.Encode( headerValue, ref formatterContext);
				_lengthManager.WriteLengthTrailer( header, headerValue.Length,
					_encoder.GetEncodedLength( headerValue.Length),
					ref formatterContext);
			}
		}

		/// <summary>
		/// Analiza la información contenida en un contexto de analisis y
		/// construcción de mensajes, y construye en base a ella un nuevo
		/// cabezal para el que el formateador se ha construido.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de analisis y construcción y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El nuevo cabezal contruido a partir de la información contenida en
		/// el contexto de análisis y construcción de mensajes.
		/// </returns>
		public virtual MessageHeader Parse( ref ParserContext parserContext) {

			// If zero, at this moment the length hasn't been decoded.
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
			StringMessageHeader header;
			if ( _padding == null) {
				header = new StringMessageHeader( _encoder.Decode(
					ref parserContext, parserContext.DecodedLength));
			} else {
				header = new StringMessageHeader( _padding.RemovePad(
					_encoder.Decode( ref parserContext,
					parserContext.DecodedLength)));
			}

			_lengthManager.ReadLengthTrailer( ref parserContext);

			return header;
		}
		#endregion
	}
}