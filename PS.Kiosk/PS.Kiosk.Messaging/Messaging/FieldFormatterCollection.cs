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

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Es el delegado para el evento que informa sobre la adición de un
	/// formateador de mensajes a la colección.
	/// </summary>
	public delegate void FieldFormatterAddedEventHandler(
		object sender, FieldFormatterEventArgs e);

	/// <summary>
	/// Es el delegado para el evento que informa sobre la eliminación de un
	/// formateador de mensajes a la colección.
	/// </summary>
	public delegate void FieldFormatterRemovedEventHandler(
		object sender, FieldFormatterEventArgs e);

	/// <summary>
	/// Es el delegado para el evento que informa que todos los formateadores
	/// de campo, han sido eliminados de la colección en una única operación.
	/// </summary>
	public delegate void FieldFormatterClearedEventHandler(
		object sender, EventArgs e);

	/// <summary>
	/// Implementa una colección de formateadores de campos.
	/// </summary>
	/// <remarks>El número de campo es empleado como clave dentro de la
	/// colección.</remarks>
	public class FieldFormatterCollection : IEnumerable {

		/// <summary>
		/// Informa que se ha agregado un formateador de mensajes a la colección.
		/// </summary>
		public event FieldFormatterAddedEventHandler Added; 

		/// <summary>
		/// Informa que se ha eliminado un formateador de mensajes de la colección.
		/// </summary>
		public event FieldFormatterRemovedEventHandler Removed; 

		/// <summary>
		/// Informa que se han eliminado todos los formateador de mensajes de la
		/// colección.
		/// </summary>
		public event FieldFormatterClearedEventHandler Cleared; 

		private Hashtable _fieldsFormatters;
		private int _maxField;
		private bool _maxFieldDirty;

		#region Constructors
		/// <summary>
		/// Crea una nueva instancia de la colección de formateadores de campo.
		/// </summary>
		public FieldFormatterCollection() {

			_fieldsFormatters = new Hashtable( 64);
			_maxField = int.MinValue;
			_maxFieldDirty = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna un formateador de campo en la colección de
		/// formateadores de campos.
		/// </summary>
		/// <remarks>
		/// Si el formateador de campo no existe en la colección,
		/// un valor nulo es retornado.
		/// </remarks>
		public FieldFormatter this[int fieldNumber] {

			get {

				return ( FieldFormatter)_fieldsFormatters[fieldNumber];
			}
		}

		/// <summary>
		/// Retorna la cantidad de formateadores de campo incluidos en la
		/// colección.
		/// </summary>
		public int Count {

			get {

				return _fieldsFormatters.Count;
			}
		}

		/// <summary>
		/// Retorna el número del máximo formateador de campo.
		/// </summary>
		/// <exception cref="ApplicationException">
		/// La colección se encuentra vacía.
		/// </exception>
		public int MaximumFieldFormatterNumber {

			get {

				if ( _fieldsFormatters.Count == 0) {
					throw new ApplicationException( SR.TheCollectionIsEmpty);
				}

				if ( _maxFieldDirty) {

					_maxField = int.MinValue;

					foreach( DictionaryEntry fieldFormatter in _fieldsFormatters) {
						if ( ( ( FieldFormatter)( fieldFormatter.Value)).FieldNumber > _maxField) {
							_maxField = ( ( FieldFormatter)( fieldFormatter.Value)).FieldNumber;
						}
					}

					_maxFieldDirty = false;
				}

				return _maxField;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Agrega un formateador de campo a la colección.
		/// </summary>
		/// <param name="fieldFormatter">Es el formateador de campo a agregar.
		/// </param>
		/// <remarks>Si existe es reemplazado.</remarks>
		public void Add( FieldFormatter fieldFormatter) {

			if ( _fieldsFormatters.Contains( fieldFormatter.FieldNumber)) {
				Remove( fieldFormatter.FieldNumber);
			}

			_fieldsFormatters[fieldFormatter.FieldNumber] = fieldFormatter;

			if ( Added != null) {
				Added( this, new FieldFormatterEventArgs( fieldFormatter));
			}

			if ( _maxField < fieldFormatter.FieldNumber) {
				_maxField = fieldFormatter.FieldNumber;
			}
		}

		/// <summary>
		/// Elimina el formateador de campo cuyo número coincida con el
		/// especificado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número de campo del formateador de campo que se desea
		/// eliminar de la colección.
		/// </param>
		public void Remove( int fieldNumber) {

			FieldFormatter fieldFormatter = null;

			if ( Removed != null) {
				if ( _fieldsFormatters.Contains( fieldNumber)) {
					fieldFormatter = this[fieldNumber];
				}
			}

			_fieldsFormatters.Remove( fieldNumber);

			if ( fieldFormatter != null) {
				Removed( this, new FieldFormatterEventArgs( fieldFormatter));
			}

			if ( fieldNumber == _maxField) {
				_maxFieldDirty = true;
			}
		}

		/// <summary>
		/// Elimina los formateadores de campo cuyos números coincidan con los
		/// indicados.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Es el conjunto de números de formateadores de campo a eliminar de la
		/// colección.
		/// </param>
		public void Remove( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				Remove( fieldsNumbers[i]);
			}
		}

		/// <summary>
		/// Elimina todos los formateadores de campo de la colección.
		/// </summary>
		public void Clear() {

			if ( _fieldsFormatters.Count == 0) {
				return;
			}

			if ( Cleared != null) {
				Cleared( this, EventArgs.Empty);
			}

			_fieldsFormatters.Clear();
			_maxField = int.MinValue;
		}

		/// <summary>
		/// Indica si la colección contiene un formateador de campo para el
		/// número de campo indicado.
		/// </summary>
		/// <param name="fieldNumber">Es el número del campo para el que se
		/// desea conocer si existe un formateador de campo en la colección.
		/// </param>
		/// <returns><see langref="true"/> si el formateador de campo está
		/// contenido en la colección, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( int fieldNumber) {

			return _fieldsFormatters.Contains( fieldNumber);
		}

		
		/// <summary>
		/// Indica si la colección contiene todos los formateadores de campo
		/// cuyos números se indican.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Son los números de formateadores de campo para los que se desea
		/// conocer si existen en la colección.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si todos los formateadores de campo están
		/// contenidos en la colección, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				if ( !Contains( fieldsNumbers[i])) {
					return false;
				}
			}

			return true;
		}
		#endregion

		#region Implementation of IEnumerable
		/// <summary>
		/// Devuelve un enumerador de la colección.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() {

			return new FieldFormattersEnumerator( _fieldsFormatters);
		}

		/// <summary>
		/// Implementa el enumerador de la colección.
		/// </summary>
		private class FieldFormattersEnumerator : IEnumerator {

			private IEnumerator _fieldFormattersEnumerator;

			#region Constructors
			/// <summary>
			/// Crea una nueva instancia de la clase <see cref="FieldFormattersEnumerator"/>.
			/// </summary>
			/// <param name="fieldFormatters">
			/// Es la tabla de hash que contiene los formateadores de campo.
			/// </param>
			public FieldFormattersEnumerator( Hashtable fieldFormatters) {

				_fieldFormattersEnumerator = fieldFormatters.GetEnumerator();
			}
			#endregion

			#region Implementation of IEnumerator
			/// <summary>
			/// Reinicia la enumeración.
			/// </summary>
			public void Reset() {
			
				_fieldFormattersEnumerator.Reset();
			}

			/// <summary>
			/// Se mueve al siguiente elemento en la enumeración.
			/// </summary>
			/// <returns>
			/// Un valor verdadero si logró posicionarse en el siguiente elemento de
			/// la enumeración, un valor igual a falso cuando no existen mas elementos
			/// a enumerar.
			/// </returns>
			public bool MoveNext() {

				return _fieldFormattersEnumerator.MoveNext();
			}

			/// <summary>
			/// Retorna el elemento actual de la enumeración.
			/// </summary>
			public object Current {

				get {

					return ( ( DictionaryEntry)_fieldFormattersEnumerator.Current).Value;
				}
			}
			#endregion
		}
		#endregion
	}
}
