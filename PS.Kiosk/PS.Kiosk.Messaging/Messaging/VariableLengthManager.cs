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
using PS.Kiosk.Messaging.Operations;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa la clase que permite administrar largos variables de
	/// datos.
	/// </summary>
	public class VariableLengthManager : LengthManager {

		private ILengthEncoder _lengthEncoder;
		private int _minimumLength;

		#region Constructors
		/// <summary>
		/// Construye un nuevo administrador de largos variable.
		/// </summary>
		/// <param name="minimumLength">
		/// Es el largo mínimo que deben tener los datos.
		/// </param>
		/// <param name="maximumLength">
		/// Es el largo máximo que pueden alcanzar los datos.
		/// </param>
		/// <param name="lengthEncoder">
		/// Es el codificador/decodificador del largo.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Se ha indicado un valor incorrecto para el largo mínimo
		/// de los datos.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Se ha indicado un codificador/decodificador inválido.
		/// </exception>
		public VariableLengthManager( int minimumLength, int maximumLength,
			ILengthEncoder lengthEncoder) : base( maximumLength) {

			if ( minimumLength < 0) {
				throw new ArgumentOutOfRangeException( "minimumLength",
					minimumLength, SR.CantBeLowerThanZero);
			}

			if ( minimumLength > maximumLength) {
				throw new ArgumentOutOfRangeException( "minimumLength",
					minimumLength, SR.CantBeGreaterThanMaximumLength);
			}

			if ( lengthEncoder == null) {
				throw new ArgumentNullException( "lengthEncoder",
					SR.LengthEncoderRequired);
			}

			_lengthEncoder = lengthEncoder;
			_minimumLength = minimumLength;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el codificador/decodificador del largo.
		/// </summary>
		public ILengthEncoder LengthEncoder {

			get {

				return _lengthEncoder;
			}
		}

		/// <summary>
		/// Retorna el largo mínimo de los datos.
		/// </summary>
		public int MinimumLength {

			get {

				return _minimumLength;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Escribe el largo de los datos del campo en el contexto de
		/// formateo del mensaje.
		/// </summary>
		/// <param name="component">
		/// Es el componente de mensajería del que se está escribiendo
		/// el largo de sus datos.
		/// </param>
		/// <param name="dataLength">
		/// Es el largo de los datos del componente (puede diferir del largo
		/// de los datos obtenidos de <paramref name="component"/>, pues pueden
		/// estar rellenados)
		/// </param>
		/// <param name="encodedLength">
		/// Es el largo de los datos codificados.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo del mensaje.
		/// </param>
		public override void WriteLength( MessagingComponent component,
			int dataLength, int encodedLength, ref FormatterContext formatterContext) {

			if ( ( dataLength < _minimumLength) ||
				( dataLength > MaximumLength)) {
				throw new ArgumentOutOfRangeException( "dataLength", dataLength,
					SR.Between( _minimumLength, MaximumLength));
			}

			_lengthEncoder.Encode( dataLength, ref formatterContext);
		}

		/// <summary>
		/// Indica si existen datos suficientes como para leer el largo
		/// de los datos desde el contexto de análisis y construcción de mensajes.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// <see langref="true"/> en caso de que existan datos suficientes
		/// como para leer el largo de los datos, <see langref="false"/> en caso
		/// contrario.
		/// </returns>
		public override bool EnoughData( ref ParserContext parserContext) {

			return ( parserContext.DataLength >=
				_lengthEncoder.EncodedLength);
		}

		/// <summary>
		/// Lee desde el contexto de análisis y construcción de mensajes,
		/// el largo de los datos.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El largo de los datos.
		/// </returns>
		/// <exception cref="MessagingException">
		/// El largo indicado para los datos no se encuentra
		/// entre los valores permitidos.
		/// </exception>
		public override int ReadLength( ref ParserContext parserContext) {

			int length = _lengthEncoder.Decode( ref parserContext);

			if ( ( length < _minimumLength) || ( length > MaximumLength)) {
				throw new MessagingException( SR.BetweenXAndYZReceived(
					_minimumLength, MaximumLength, length));
			}

			return length;
		}
		#endregion
	}
}
