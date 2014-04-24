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

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un formateador capaz de formatear o analizar un mapa de bits.
	/// </summary>
	public class BitMapFieldFormatter : FieldFormatter {

		private IBinaryEncoder _encoder;
		private int _lowerFieldNumber;
		private int _upperFieldNumber;
		private int _bitmapLength;

		#region Constructors
		/// <summary>
		/// Construye un nuevo formateador de mapas de bits.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz de formatear.
		/// </param>
		/// <param name="lowerFieldNumber">
		/// Es el número de campo menor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el número de campo mayor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del mapa de bits.
		/// </param>
		public BitMapFieldFormatter( int fieldNumber, int lowerFieldNumber,
			int upperFieldNumber, IBinaryEncoder encoder) : this( fieldNumber,
			lowerFieldNumber, upperFieldNumber, encoder, string.Empty) {

		}

		/// <summary>
		/// Construye un nuevo formateador de mapas de bits.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del mensaje que el formateador es capaz de formatear.
		/// </param>
		/// <param name="lowerFieldNumber">
		/// Es el número de campo menor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el número de campo mayor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="encoder">
		/// Es el objeto capaz de codificar/decodificar los datos del mapa de bits.
		/// </param>
		/// <param name="description">
		/// Es la descripción del mapa de bits.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="lowerFieldNumber"/> debe ser mayor que cero y menor
		/// o igual a <paramref name="upperFieldNumber"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// El parámetro <paramref name="encoder"/> es inválido.
		/// </exception>
		public BitMapFieldFormatter( int fieldNumber, int lowerFieldNumber,
			int upperFieldNumber, IBinaryEncoder encoder, string description) :
			base( fieldNumber, description) {
			
			if ( lowerFieldNumber < 0) {
				throw new ArgumentOutOfRangeException( "lowerFieldNumber", lowerFieldNumber,
					SR.CantBeLowerThanZero);
			}

			if ( lowerFieldNumber > upperFieldNumber) {
				throw new ArgumentOutOfRangeException( "lowerFieldNumber", lowerFieldNumber,
					SR.MustBeLowerOrEqualToUpperFieldNumber);
			}

			if ( encoder == null) {
				throw new ArgumentNullException( "encoder");
			}

			_lowerFieldNumber = lowerFieldNumber;
			_upperFieldNumber = upperFieldNumber;
			_encoder = encoder;
			_bitmapLength = ( ( _upperFieldNumber - _lowerFieldNumber) + 8) / 8;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Es el número de campo menor que el mapa de bits puede anunciar.
		/// </summary>
		public int LowerFieldNumber {

			get {

				return _lowerFieldNumber;
			}
		}

		/// <summary>
		/// Es el número de campo mayor que el mapa de bits puede anunciar.
		/// </summary>
		public int UpperFieldNumber {

			get {

				return _upperFieldNumber;
			}
		}

		/// <summary>
		/// Retorna el codificar/decodificar de los datos del mapa de bits.
		/// </summary>
		public IBinaryEncoder Encoder {

			get {

				return _encoder;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Formatea un mapa de bits.
		/// </summary>
		/// <param name="field">
		/// Es el campo a formatear.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo que debe ser empleado.
		/// </param>
		public override void Format( Field field,
			ref FormatterContext formatterContext) {

			if ( !( field is BitMapField)) {
				throw new ArgumentException( SR.FieldMustBeABitmap, "field");
			}

			_encoder.Encode( ( ( BitMapField)field).GetBytes(), ref formatterContext);
		}

		/// <summary>
		/// Analiza la información contenida en un contexto de analisis y construcción
		/// de mensajes, y construye en base a ella un nuevo campo para el que el
		/// formateador se ha construido.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de analisis y construcción y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El nuevo campo contruido a partir de la información contenida en el contexto
		/// de análisis y construcción de mensajes.
		/// </returns>
		public override Field Parse( ref ParserContext parserContext) {

			if ( parserContext.DataLength < _encoder.GetEncodedLength( _bitmapLength)) {
				// Insufficient data to parse bitmap, return null.
				return null;
			}

			return new BitMapField( FieldNumber, _lowerFieldNumber, _upperFieldNumber,
				_encoder.Decode( ref parserContext, _bitmapLength));
		}
		#endregion
	}
}
