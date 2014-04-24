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
using System.Text;
using Fanap.Utilities;
using PS.Kiosk.Messaging.Operations;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa el contexto utilizado para formatear mensajes.
	/// </summary>
	public struct FormatterContext {

		/// <summary>
		/// En caso de que el buffer necesite ser ampliado, será ampliado la cantidad de
		/// bytes que esta constante indica.
		/// </summary>
		public const int DefaultBufferSize = 2048;

		private byte[] _buffer;
		private int _upperDataBound;
		private Message _currentMessage;

		#region Constructors
		/// <summary>
		/// Construye un nuevo contexto para el formateo de mensajes.
		/// </summary>
		/// <param name="bufferSize">
		/// Es el tamaño inicial del buffer donde residirán los datos formateados.
		/// </param>
		/// <exception cref="ArgumentException">
		/// En caso de que <paramref name="bufferSize"/> sea menor a 1.
		/// </exception>
		public FormatterContext( int bufferSize) {

			if ( bufferSize < 1) {
				throw new ArgumentException( SR.MustBeGreaterThanZero, "bufferSize");
			}

			_buffer = new byte[bufferSize];
			_upperDataBound = 0;
			_currentMessage = null;
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns or sets the current message.
		/// </summary>
		public Message CurrentMessage {

			get {

				return _currentMessage;
			}

			set {

				_currentMessage = value;
			}
		}

		/// <summary>
		/// Retorna o asigna el puntero dentro del buffer que indica hasta donde
		/// se han almacenado los datos formateados del mensaje.
		/// </summary>
		public int UpperDataBound {

			get {

				return _upperDataBound;
			}

			set {

				_upperDataBound = value;
			}
		}

		/// <summary>
		/// Retorna el espacio disponible en el buffer donde residirán los datos
		/// formateados.
		/// </summary>
		public int FreeBufferSpace {

			get {

				return _buffer.Length - _upperDataBound;
			}
		}

		/// <summary>
		/// Retorna el tamaño del buffer.
		/// </summary>
		public int BufferSize {

			get {

				return _buffer.Length;
			}
		}

		/// <summary>
		/// Retorna la cantidad de bytes disponibles en el buffer de datos
		/// formateados.
		/// </summary>
		public int DataLength {

			get {

				return _upperDataBound;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna el buffer donde residen los datos formateados.
		/// </summary>
		/// <returns>
		/// Retorna el buffer donde residen los datos formateados.
		/// </returns>
		public byte[] GetBuffer() {

			return _buffer;
		}

		/// <summary>
		/// Aumenta el tamaño del buffer en al menos la cantidad de bytes
		/// indicada.
		/// </summary>
		/// <param name="count">
		/// Es la cantidad mínima de bytes en que al menos se agranda el
		/// buffer.
		/// </param>
		/// <remarks>
		/// Este método amplía el buffer en bloques cuyo tamaño está definido
		/// por <see cref="DefaultBufferSize"/>.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">
		/// En caso de que <paramref name="count"/> sea menor a 1.
		/// </exception>
		public void ResizeBuffer( int count) {

			if ( count < 1) {
				throw new ArgumentOutOfRangeException( "count",
					count, SR.MustBeGreaterThanZero);
			}

			// Save current buffer.
			byte[] buffer = _buffer;
			int dataLength = DataLength;

			// Resize buffer.
			_buffer = new byte[_buffer.Length +
				( ( ( count % DefaultBufferSize) == 0) ? count :
				( ( ( count / DefaultBufferSize) + 1) * DefaultBufferSize))];

			Clear();

			// Copy data.
			if ( dataLength > 0) {
				Write( buffer, 0, dataLength);
			}
		}

		/// <summary>
		/// Elimina los datos formateados contenidos en el buffer.
		/// </summary>
		public void Clear() {

			_upperDataBound = 0;
		}

		/// <summary>
		/// Elimina los datos formateados contenidos en el buffer,
		/// reservando al principio la cantidad de bytes indicada.
		/// </summary>
		public void Clear( int reserve) {

			if ( reserve < 1) {
				throw new ArgumentOutOfRangeException( "reserve",
					reserve, SR.MustBeGreaterThanZero);
			}

			// Check if we must resize our buffer.
			if ( _buffer.Length < reserve) {
				// Resize buffer.
				_buffer = new byte[_buffer.Length +
					( ( ( reserve % DefaultBufferSize) == 0) ? reserve :
					( ( ( reserve / DefaultBufferSize) + 1) * DefaultBufferSize))];
			}

			_upperDataBound = reserve;
		}

		/// <summary>
		/// Escribe en el buffer de datos formateados, los datos indicados.
		/// </summary>
		/// <param name="data">
		/// Son los datos a escribir en el buffer.
		/// </param>
		public void Write( string data) {

			Write( data, 0, data.Length);
		}

		/// <summary>
		/// Escribe en el buffer de datos formateados, los datos indicados.
		/// </summary>
		/// <param name="data">
		/// Son los datos a escribir en el buffer.
		/// </param>
		/// <param name="offset">
		/// Indica a partir desde donde se comienza a copiar la información
		/// contenida en <paramref name="data"/>.
		/// </param>
		/// <param name="count">
		/// Indica la cantidad de caracteres a copiar.
		/// </param>
		public void Write( string data, int offset, int count) {

			if ( offset > data.Length) {
				throw new ArgumentException( SR.OutOfBound, "offset");
			}

			if ( ( offset + count) > data.Length) {
				throw new ArgumentException( SR.OutOfBound, "count");
			}

			// Check if we must resize our buffer.
			if ( FreeBufferSpace < count) {
				ResizeBuffer( count);
			}

			// Write data.
			for ( int i = 0; i < count; i++) {
				_buffer[_upperDataBound + i] = ( byte)( data[offset + i]);
			}

			// Update upper data bound.
			_upperDataBound += count;
		}

		/// <summary>
		/// Escribe en el buffer de datos formateados, los datos indicados.
		/// </summary>
		/// <param name="data">
		/// Son los datos a escribir en el buffer.
		/// </param>
		public void Write( byte[] data) {

			Write( data, 0, data.Length);
		}

		/// <summary>
		/// Escribe en el buffer de datos formateados, los datos indicados.
		/// </summary>
		/// <param name="data">
		/// Son los datos a escribir en el buffer.
		/// </param>
		/// <param name="offset">
		/// Indica a partir desde donde se comienza a copiar la información
		/// contenida en <paramref name="data"/>.
		/// </param>
		/// <param name="count">
		/// Indica la cantidad de bytes a copiar.
		/// </param>
		public void Write( byte[] data, int offset, int count) {

			if ( offset > data.Length) {
				throw new ArgumentException( SR.OutOfBound, "offset");
			}

			if ( ( offset + count) > data.Length) {
				throw new ArgumentException( SR.OutOfBound, "count");
			}

			// Check if we must resize our buffer.
			if ( FreeBufferSpace < count) {
				ResizeBuffer( count);
			}

			// Write data.
			for ( int i = 0; i < count; i++) {
				_buffer[_upperDataBound + i] = data[offset + i];
			}

			// Update upper data bound.
			_upperDataBound += count;
		}

		/// <summary>
		/// Devuelve un array de bytes conteniendo una copia de los datos
		/// almacenados en el buffer.
		/// </summary>
		/// <returns>
		/// Una copia de los datos almacenados en el buffer.
		/// </returns>
		/// <remarks>
		/// Si el buffer no contiene datos, esta función retorna
		/// <see langref="null"/>.
		/// </remarks>
		public byte[] GetData() {

			if ( _upperDataBound == 0) {
				return null;
			}

			byte[] data = new byte[_upperDataBound];

			for ( int i = 0; i < _upperDataBound; i++) {
				data[i] = _buffer[i];
			}

			return data;
		}

		/// <summary>
		/// Devuelve una cadena de caracteres conteniendo una copia de
		/// los datos almacenados en el buffer.
		/// </summary>
		/// <returns>
		/// Una copia de los datos almacenados en el buffer.
		/// </returns>
		/// <remarks>
		/// Si el buffer no contiene datos, esta función retorna
		/// <see langref="null"/>.
		/// </remarks>
		public string GetDataAsString() {

			if ( _upperDataBound == 0) {
				return null;
			}

            return FrameworkEncoding.GetInstance().Encoding.GetString( _buffer, 0, _upperDataBound );
		}

		/// <summary>
		/// Initializes the context.
		/// </summary>
		public void Initialize() {

			Clear();
		}
		#endregion
	}
}
