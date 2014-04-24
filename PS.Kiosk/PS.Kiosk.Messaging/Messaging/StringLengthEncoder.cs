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
	/// Implementa un codificador de indicadores de largos de valores de
	/// componentes de mensajería, utilizando como formato de datos el set
	/// de caracteres ASCII.
	/// </summary>
	/// <remarks>
	/// Normalmente se emplean indicadores de largo, cuando los datos de
	/// los componentes de mensajería son de largo variable.
	/// This class implements the Singleton pattern, you must use
	/// <see cref="GetInstance"/> to acquire the instance.
	/// </remarks>
	public class StringLengthEncoder : ILengthEncoder {

		// One for each supported size, if more are required only
		// enlarge the instances array.
		private static volatile StringLengthEncoder[] _instances = {
			null, null, null, null, null, null
		};
		private static int[] _lengths = { 9, 99, 999, 9999, 99999, 999999};

		private int _lengthsIndex;

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia del codificador. Le damos el nivel
		/// del visibilidad 'private' para forzar al usuario a emplear
		/// <see cref="GetInstance"/>.
		/// </summary>
		/// <param name="lengthsIndex">
		/// Es el índice dentro del array _lengths que guarda la información
		/// del largo máximo que se puede codificar.
		/// </param>
		private StringLengthEncoder( int lengthsIndex) {

			_lengthsIndex = lengthsIndex;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Es el largo máximo que se puede codificar.
		/// </summary>
		public int MaximumLength {

			get {

				return _lengths[_lengthsIndex];
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una instancia de la clase <see cref="StringLengthEncoder"/>
		/// configurada de acuerdo a los requerimientos especificados.
		/// </summary>
		/// <param name="maximumLength">
		/// Es el largo máximo que se puede codificar.
		/// </param>
		/// <returns>
		/// Una instancia de la clase <see cref="StringLengthEncoder"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Cuando <paramref name="maximumLength"/> contiene un valor inválido.
		/// </exception>
		public static StringLengthEncoder GetInstance( int maximumLength) {

			if ( maximumLength < 0) {
				throw new ArgumentOutOfRangeException( "maximumLength", maximumLength,
					SR.CantBeLowerThanZero);
			}

			if ( maximumLength > _lengths[_lengths.Length - 1]) {
				throw new ArgumentOutOfRangeException( "maximumLength", maximumLength,
					SR.OnlyZeroToNAllowed( _lengths[_lengths.Length - 1]));
			}

			int index = 0;
			for ( ; index < _lengths.Length; index++) {
				if ( maximumLength <= _lengths[index]) {
					break;
				}
			}

			if ( _instances[index] == null) {
				lock ( typeof( StringLengthEncoder)) {
					if ( _instances[index] == null) {
						_instances[index] = new StringLengthEncoder( index);
					}
				}
			}

			return _instances[index];
		}
		#endregion

		#region ILengthEncoder Members
		/// <summary>
		/// Retorna el largo que ocupa el indicador de largo.
		/// </summary>
		public int EncodedLength {

			get {

				return _lengthsIndex + 1;
			}
		}

		/// <summary>
		/// Formatea el largo de los datos del componente de mensajería.
		/// </summary>
		/// <param name="length">
		/// Es el largo de los datos del componente de mensajería.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo donde se almacenará la
		/// información formateada.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Cuando <paramref name="length"/> sobrepasa el valor
		/// máximo permitido.
		/// </exception>
		public void Encode( int length, ref FormatterContext formatterContext) {

			if ( length > _lengths[_lengthsIndex]) {
				throw new ArgumentOutOfRangeException( "length", length,
					SR.LessOrEqualToN( _lengths[_lengthsIndex]));
			}

			// Check if we must resize our buffer.
			if ( formatterContext.FreeBufferSpace < ( _lengthsIndex + 1)) {
				formatterContext.ResizeBuffer( _lengthsIndex + 1);
			}

			byte[] buffer = formatterContext.GetBuffer();

			// Write encoded length.
			for ( int i = formatterContext.UpperDataBound + _lengthsIndex;
				i >= formatterContext.UpperDataBound; i--) {
				buffer[i] = ( byte)( length % 10 + 0x30);
				length /= 10;
			}

			// Update formatter context upper data bound.
			formatterContext.UpperDataBound += _lengthsIndex + 1;
		}

		/// <summary>
		/// Convierte los datos formateados del indicador de largo
		/// de los datos del componente de mensajería.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes donde
		/// reside la información a decodificar.
		/// </param>
		/// <returns>
		/// Es el largo de los datos del componente de mensajería.
		/// </returns>
		public int Decode( ref ParserContext parserContext) {

			// Check available data.
			if ( parserContext.DataLength < ( _lengthsIndex + 1)) {
				throw new ArgumentException( SR.InsufficientData, "parserContext");
			}

			int length = 0;
			byte[] buffer = parserContext.GetBuffer();
			int offset = parserContext.LowerDataBound;

			// Decode length.
			for ( int i = offset; i < ( offset + _lengthsIndex + 1); i++) {

				if ( ( buffer[i] < 0x30) || ( buffer[i] > 0x39)) {
					throw new MessagingException( SR.InvalidByteLengthDetected( buffer[i]));
				}

				length = length * 10 + buffer[i] - 0x30;
			}

			// Consume parser context data.
			parserContext.Consumed( _lengthsIndex + 1);

			return length;
		}
		#endregion
	}
}
