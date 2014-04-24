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
using System.Collections;
using System.Text;
using Fanap.Utilities;
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un campo de mensajes que es un mapa de bits.
	/// </summary>
	/// <remarks>
	/// Normalmente es utilizado para informar cuales campos están presentes
	/// en un mensaje, con el objetivo de que el parser lo pueda analizar.
	/// Gracias a los mapas de bits es posible generar mensajes cuyos campos
	/// pueden o no estar presentes.
	/// </remarks>
    [Serializable]
	public class BitMapField : Field {

		private byte[] _value;
		private int _lowerFieldNumber;
		private int _upperFieldNumber;

		#region Constructors
		/// <summary>
		/// Construye un nuevo mapa de bits.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo en el mensaje.
		/// </param>
		/// <param name="lowerFieldNumber">
		/// Es el número de campo menor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el número de campo mayor que el mapa de bits puede anunciar.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="lowerFieldNumber"/> debe ser mayor que cero y menor
		/// o igual a <paramref name="upperFieldNumber"/>.
		/// </exception>
		public BitMapField( int fieldNumber, int lowerFieldNumber,
			int upperFieldNumber) : base( fieldNumber) {

			if ( lowerFieldNumber < 0) {
				throw new ArgumentOutOfRangeException( "lowerFieldNumber", lowerFieldNumber,
					SR.CantBeLowerThanZero);
			}

			if ( lowerFieldNumber > upperFieldNumber) {
				throw new ArgumentOutOfRangeException( "lowerFieldNumber", lowerFieldNumber,
					SR.MustBeLowerOrEqualToUpperFieldNumber);
			}

			_lowerFieldNumber = lowerFieldNumber;
			_upperFieldNumber = upperFieldNumber;

			_value = new byte[ ( ( upperFieldNumber - lowerFieldNumber) + 8) / 8];

			for ( int i = 0; i < _value.Length; i++) {
				_value[i] = 0;
			}
		}

		/// <summary>
		/// Construye un nuevo mapa de bits.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo en el mensaje.
		/// </param>
		/// <param name="lowerFieldNumber">
		/// Es el número de campo menor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el número de campo mayor que el mapa de bits puede anunciar.
		/// </param>
		/// <param name="value">
		/// Es el valor que toma el mapa de bits.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="lowerFieldNumber"/> debe ser menor o igual a
		/// <paramref name="upperFieldNumber"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// La cantidad de bytes del valor que debe tomar el nuevo mapa de bits
		/// no concuerda con los que se calculan en base a los parámetros
		/// <paramref name="lowerFieldNumber"/> y <paramref name="upperFieldNumber"/>.
		/// </exception>
		public BitMapField( int fieldNumber, int lowerFieldNumber,
			int upperFieldNumber, byte[] value) : base( fieldNumber) {

			if ( lowerFieldNumber > upperFieldNumber) {
				throw new ArgumentOutOfRangeException( "lowerFieldNumber", lowerFieldNumber,
					SR.MustBeLowerOrEqualToUpperFieldNumber);
			}

			if ( value.Length != ( ( ( upperFieldNumber - lowerFieldNumber) + 8) / 8)) {
				throw new ArgumentException( SR.UnexpectedLength, "value");
			}

			_lowerFieldNumber = lowerFieldNumber;
			_upperFieldNumber = upperFieldNumber;

			_value = value;
		}

		/// <summary>
		/// Construye un mapa de bits exactamente igual a otro dado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo en el mensaje.
		/// </param>
		/// <param name="bitmap">
		/// Es el mapa de bits que se toma como referencia para crear uno nuevo.
		/// </param>
		public BitMapField( int fieldNumber, BitMapField bitmap) : base( fieldNumber) {

			_lowerFieldNumber = bitmap.LowerFieldNumber;
			_upperFieldNumber = bitmap.UpperFieldNumber;

			byte[] bitmapValue = bitmap.GetBytes();

			_value = new byte[bitmapValue.Length];

			for ( int i = 0; i < _value.Length; i++) {
				_value[i] = bitmapValue[i];
			}
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
		/// Retorna o asigna el valor del bit que representa al campo indicado.
		/// </summary>
		public bool this[int fieldNumber] {

			get {

				return IsSet( fieldNumber);
			}

			set {

				Set( fieldNumber, value);
			}
		}

		/// <summary>
		/// Retorna o asigna el valor del mapa de bits.
		/// </summary>
		public override object Value {

			get {

				return _value;
			}

			set {
			
				if ( value is byte[]) {
					SetFieldValue( ( byte[])value);
				} else if ( value is string) {
					if ( ( ( string)value).Length != _value.Length) {
						throw new ArgumentException( SR.InvalidLength, "value");
					}

                    _value = FrameworkEncoding.GetInstance().Encoding.GetBytes( ( string )value );
				} else {
					throw new ArgumentException( SR.CantHandleParameterType, "value");
				}
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Asigna el valor del campo.
		/// </summary>
		public void SetFieldValue( byte[] value) {

			if ( value.Length != _value.Length) {
				throw new ArgumentException( SR.InvalidLength, "value");
			}

			_value = value;
		}

		/// <summary>
		/// Asigna un valor al bit que representa el campo indicado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el bit que representa al campo indicado por el parámetro.
		/// </param>
		/// <param name="value">
		/// Es el valor a asignar.
		/// </param>
		public void Set( int fieldNumber, bool value) {

			if ( ( fieldNumber < _lowerFieldNumber) || ( fieldNumber > _upperFieldNumber)) {
				throw new ArgumentOutOfRangeException( "fieldNumber", fieldNumber,
					SR.InvalidFieldNumberParameter( _lowerFieldNumber, _upperFieldNumber));
			}

			fieldNumber -= _lowerFieldNumber;

			if ( value) {
				_value[fieldNumber / 8] |= ( byte)( 1 << ( 7 - ( fieldNumber % 8)));
			} else {
				_value[fieldNumber / 8] &= ( byte)( ~( 1 << ( 7 - ( fieldNumber % 8))));
			}
		}

		/// <summary>
		/// Asigna <see langref="false"/> a todos los bits del bitmap.
		/// </summary>
		public void Clear() {

			for ( int i = 0; i < _value.Length; i++) {
				_value[i] = 0;
			}
		}

		/// <summary>
		/// Consulta el valor del bit que representa el campo indicado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el bit que representa al campo indicado por el parámetro.
		/// </param>
		/// <returns>
		/// El valor del bit.
		/// </returns>
		public bool IsSet( int fieldNumber) {

			if ( ( fieldNumber < _lowerFieldNumber) || ( fieldNumber > _upperFieldNumber)) {
				throw new ArgumentOutOfRangeException( "fieldNumber", fieldNumber,
					SR.InvalidFieldNumberParameter( _lowerFieldNumber, _upperFieldNumber));
			}

			fieldNumber -= _lowerFieldNumber;
    
			return ( _value[fieldNumber / 8] & ( 1 << ( 7 - ( fieldNumber % 8)))) != 0;
		}

		/// <summary>
		/// Retorna como un array de bytes el valor del mapa de bits.
		/// </summary>
		/// <returns>
		/// Un array de bytes.
		/// </returns>
		public override byte[] GetBytes() {

			return _value;
		}

		/// <summary>
		/// Construye una copia exacta del mapa de bits.
		/// </summary>
		/// <returns>
		/// Una copia exacta del mapa de bits.
		/// </returns>
		public override object Clone() {

			return new BitMapField( FieldNumber, this);
		}

		/// <summary>
		/// Convierte en una cadena de caracteres el valor del mapa de bits.
		/// </summary>
		/// <returns>
		/// Una cadena de caracteres que representan el valor del mapa de bits.
		/// </returns>
		public override string ToString() {

			StringBuilder rendered = new StringBuilder();
			bool comma = false;

			rendered.Append( "[");
			for( int i = _lowerFieldNumber; i <= _upperFieldNumber; i++) {
				if ( IsSet( i)) {
					if ( comma) {
						rendered.Append( string.Format( ",{0}", i));
					} else {
						comma = true;
						rendered.Append( i);
					}
				}
			}
			rendered.Append( "]");

			return rendered.ToString();
		}

		/// <summary>
		/// Retorna una clase que puede representar en formato XML el mapa de bits.
		/// </summary>
		/// <param name="renderingMap">
		/// Es un mapa con todas las clases que representan objetos.
		/// </param>
		/// <returns>
		/// Una clase que puede representar en formato XML el mapa de bits.
		/// </returns>
		public override MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap) {

			IObjectRenderer objectRendering = renderingMap.Get( typeof( BitMapField));

			if ( objectRendering == null) {
				// Add renderer to map.
				objectRendering = new BitMapFieldXmlRendering();
				renderingMap.Put( typeof( BitMapField), objectRendering);
			} else
				if ( !( objectRendering is BitMapFieldXmlRendering))
				objectRendering = new BitMapFieldXmlRendering();

			return ( MessagingComponentXmlRendering)objectRendering;
		}

		/// <summary>
		/// Crea un nuevo componente de mensajería de tipo mapa de bits.
		/// </summary>
		/// <returns>
		/// Un nuevo componente de mensajería de tipo mapa de bits.
		/// </returns>
		public override MessagingComponent NewComponent() {

			return new BitMapField( FieldNumber, _lowerFieldNumber, _upperFieldNumber);
		}
		#endregion
	}
}
